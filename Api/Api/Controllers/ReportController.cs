using Api.Application.DTOs;
using Api.Application.Services;
using Api.Domain.Entities;
using Api.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(ReportService service) : ControllerBase
    {
        [Authorize(Policy = "AdminActive")]
        [HttpPost("")]
        public async Task<IActionResult> Report([FromBody] CreateReportDto dto)
        {
            Console.WriteLine(JsonSerializer.Serialize(dto));
            var data = await service.Create(dto);
            var response = DefaultResponse.SendOk<Report>("Todo correcto", data);
            return Ok(response);
        }

        [Authorize(Policy = "AdminActive")]
        [HttpGet("")]
        public async Task<IActionResult> FindAll()
        {
            var data = await service.FindAllTree();
            var response = DefaultResponse.SendOk<List<ReportWithProductDto>>("Todo correcto", data);
            return Ok(response);
        }
    }
}
