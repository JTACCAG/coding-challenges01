namespace Api.Application.Exceptions
{
    public class HttpException : Exception
    {
        public int StatusCode { get; }

        protected HttpException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public class BadRequestException : HttpException
    {
        public BadRequestException(string message)
            : base(StatusCodes.Status400BadRequest, message) { }
    }

    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException(string message = "No autorizado")
            : base(StatusCodes.Status401Unauthorized, message) { }
    }

    public class ForbiddenException : HttpException
    {
        public ForbiddenException(string message = "Acceso denegado")
            : base(StatusCodes.Status403Forbidden, message) { }
    }

    public class NotFoundException : HttpException
    {
        public NotFoundException(string message)
            : base(StatusCodes.Status404NotFound, message) { }
    }

    public class ConflictException : HttpException
    {
        public ConflictException(string message)
            : base(StatusCodes.Status409Conflict, message) { }
    }

    public class UnprocessableEntityException : HttpException
    {
        public UnprocessableEntityException(string message)
            : base(StatusCodes.Status422UnprocessableEntity, message) { }
    }
}
