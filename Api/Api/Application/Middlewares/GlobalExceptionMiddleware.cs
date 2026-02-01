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

                // Mapear excepción a statusCode y mensaje
                var (statusCode, message) = ex switch
                {
                    UnauthorizedAccessException _ => (StatusCodes.Status401Unauthorized, ex.Message),
                    KeyNotFoundException _ => (StatusCodes.Status404NotFound, ex.Message),
                    ArgumentException _ => (StatusCodes.Status400BadRequest, ex.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor")
                };

                context.Response.StatusCode = statusCode;

                // Serializar JSON equivalente a JSON.stringify
                var json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    success = false,
                    statusCode,
                    message,
                    timestamp = DateTime.UtcNow
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
