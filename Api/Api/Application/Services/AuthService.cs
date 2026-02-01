using Api.Application.DTOs;
using Api.Application.Repositories;
using System.Threading.Tasks;

namespace Api.Application.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseAuthDto> Login(LoginDto dto)
        {
            var user = await _userService.GetByEmail(dto.Email);

            return new ResponseAuthDto
            {
                AccessToken = "a",
                User = user!,
            };
        }
    }
}
