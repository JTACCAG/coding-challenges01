using Api.Application.DTOs;
using Api.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var data = await authService.Login(dto);
            var response = DefaultResponse.SendOk<ResponseAuthDto>("Todo correcto", data);
            return Ok(response);
        }
    }
}
