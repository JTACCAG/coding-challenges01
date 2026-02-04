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
    public class CategoryController(CategoryService service) : ControllerBase
    {
        [Authorize(Policy = "AdminActive")]
        [HttpPost("")]
        public async Task<IActionResult> Created([FromBody] CreateCategoryDto dto)
        {
            var data = await service.Created(dto);
            var response = DefaultResponse.SendOk<Category>("Todo correcto", data);
            return Ok(response);
        }

        [Authorize(Policy = "AdminActive")]
        [HttpGet("{id}")]
        public async Task<IActionResult> FindOne(string id)
        {
            var data = await service.FindOne(id);
            var response = DefaultResponse.SendOk<Category>("Todo correcto", data);
            return Ok(response);
        }

        [Authorize(Policy = "AdminActive")]
        [HttpGet("")]
        public async Task<IActionResult> FindAll([FromQuery] SearchProductDto dto)
        {
            var data = await service.FindAll(dto);
            var response = DefaultResponse.SendOk<List<Category>>("Todo correcto", data);
            return Ok(response);
        }
    }
}
