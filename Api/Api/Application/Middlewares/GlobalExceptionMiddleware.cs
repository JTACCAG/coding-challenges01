using Api.Application.DTOs;
using System.Net;
using System.Text.Json;

namespace Api.Application.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción capturada por middleware");

                context.Response.Clear();
                context.Response.ContentType = "application/json";

                ResponseDto<object> response;

                // Mapear la excepción al tipo de respuesta adecuada
                switch (ex)
                {
                    case UnauthorizedAccessException:
                        response = DefaultResponse.SendUnauthorized<object>(ex.Message);
                        break;
                    case KeyNotFoundException:
                        response = DefaultResponse.SendNotFound<object>(ex.Message);
                        break;
                    case ArgumentException:
                        response = DefaultResponse.SendBadRequest<object>(ex.Message);
                        break;
                    default:
                        response = DefaultResponse.SendBadRequest<object>("Error interno del servidor");
                        break;
                }

                context.Response.StatusCode = response.StatusCode;

                // Serializar JSON en camelCase para que "Success" sea "success"
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true // opcional, solo para ver mejor en el navegador
                };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
