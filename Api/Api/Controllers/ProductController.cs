using Api.Application.DTOs;
using Api.Application.Services;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(ProductService service) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var data = await service.Created(dto);
            var response = DefaultResponse.SendOk<Product>("Todo correcto", data);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto dto)
        {
            var data = await service.Updated(id, dto);
            var response = DefaultResponse.SendOk<Product>("Todo correcto", data);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var data = await service.Deleted(id);
            var response = DefaultResponse.SendOk<Product>("Todo correcto", data);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindOne(string id)
        {
            var data = await service.FindOne(id);
            var response = DefaultResponse.SendOk<Product>("Todo correcto", data);
            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> FindAll([FromQuery] SearchProductDto dto)
        {
            var data = await service.FindAll(dto);
            var response = DefaultResponse.SendOk<List<Product>>("Todo correcto", data);
            return Ok(response);
        }
    }
}
