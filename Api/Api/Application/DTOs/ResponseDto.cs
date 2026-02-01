namespace Api.Application.DTOs
{
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public int StatusCode { get; set; }
    }

    public static class DefaultResponse
    {
        public static ResponseDto<T> SendBadRequest<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        public static ResponseDto<T> SendNotFound<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public static ResponseDto<T> SendForbidden<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        public static ResponseDto<T> SendCreated<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status201Created
            };
        }

        public static ResponseDto<T> SendOk<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static ResponseDto<T> SendUnauthorized<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        public static ResponseDto<T> SendConflict<T>(string message, T? data = default)
        {
            return new ResponseDto<T>
            {
                Success = false,
                Message = message,
                Data = data,
                StatusCode = StatusCodes.Status409Conflict
            };
        }
    }
}
