using Api.Application.DTOs;
using Api.Application.Enums;
using Api.Application.Repositories;
using Api.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly string _jwtKey;

        public AuthService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtKey = configuration["JwtConfig:Key"]!;
        }

        public async Task<ResponseAuthDto> Login(LoginDto dto)
        {
            var user = await _userService.GetByEmail(dto.Email) ?? throw new UnauthorizedAccessException("El correo electrónico proporcionado no se encuentra registrado en nuestro sistema");
            var isEqual = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isEqual)
                throw new UnauthorizedAccessException("La contraseña proporcionada es incorrecta. Por favor, verifica tus datos e inténtalo de nuevo");
            user.Password = null!;
            return new ResponseAuthDto
            {
                AccessToken = await GenerateToken(user, user.Role),
                User = new UserDto{
                    Id= user.Id,
                    Email = user.Email,
                    Fullname = user.Fullname,
                    Role = user.Role
                },
            };
        }

        private async Task<string> GenerateToken(User user, RoleEnum? role = null)
        {
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", (role ?? RoleEnum.Regular).ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
