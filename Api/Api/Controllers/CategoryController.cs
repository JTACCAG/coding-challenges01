using Api.Application.DTOs;
using Api.Application.Services;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(CategoryService service) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] CreateCategoryDto dto)
        {
            var data = await service.Created(dto);
            var response = DefaultResponse.SendOk<Category>("Todo correcto", data);
            return Ok(response);
        }
    }
}
