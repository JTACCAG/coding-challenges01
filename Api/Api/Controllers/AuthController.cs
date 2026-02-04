using Api.Application.DTOs;
using Api.Application.Services;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var data = await authService.Register(dto);
            var response = DefaultResponse.SendOk<ResponseAuthDto>("Todo correcto", data);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var data = await authService.Login(dto);
            var response = DefaultResponse.SendOk<ResponseAuthDto>("Todo correcto", data);
            return Ok(response);
        }
    }
}
