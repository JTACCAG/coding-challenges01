using Api.Application.DTOs;

namespace Api.Application.Services
{
    public class AuthService
    {
        public ResponseAuthDto Login(LoginDto dto)
        {
            if (dto.Email != "test@test.com" || dto.Password != "123456")
            {
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            return new ResponseAuthDto
            {
                AccessToken = "a"
            };
        }
    }
}
