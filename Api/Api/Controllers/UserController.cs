using Api.Application.DTOs;
using Api.Application.Services;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService service) : ControllerBase
    {
        [Authorize(Policy = "AdminActive")]
        [HttpGet("")]
        public async Task<IActionResult> FindAll([FromQuery] SearchUserDto dto)
        {
            var data = await service.FindAll(dto);
            var response = DefaultResponse.SendOk<List<User>>("Todo correcto", data);
            return Ok(response);
        }
    }
}
