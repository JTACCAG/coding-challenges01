using Api.Application.DTOs;
using Api.Application.Exceptions;
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
                if (ex is HttpException httpEx)
                {
                    response = httpEx.StatusCode switch
                    {
                        StatusCodes.Status400BadRequest =>
                            DefaultResponse.SendBadRequest<object>(httpEx.Message),

                        StatusCodes.Status401Unauthorized =>
                            DefaultResponse.SendUnauthorized<object>(httpEx.Message),

                        StatusCodes.Status403Forbidden =>
                            DefaultResponse.SendForbidden<object>(httpEx.Message),

                        StatusCodes.Status404NotFound =>
                            DefaultResponse.SendNotFound<object>(httpEx.Message),

                        StatusCodes.Status409Conflict =>
                            DefaultResponse.SendConflict<object>(httpEx.Message),

                        StatusCodes.Status422UnprocessableEntity =>
                            new ResponseDto<object>
                            {
                                Success = false,
                                Message = httpEx.Message,
                                Data = null,
                                StatusCode = StatusCodes.Status422UnprocessableEntity
                            },

                        _ =>
                            DefaultResponse.SendBadRequest<object>("Error no manejado")
                    };
                }
                else
                {
                    // 🟡 Excepciones del framework / no controladas
                    response = DefaultResponse.SendBadRequest<object>(
                        "Error interno del servidor"
                    );
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
