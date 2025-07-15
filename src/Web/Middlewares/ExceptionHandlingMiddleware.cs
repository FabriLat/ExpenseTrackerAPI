using Domain.Exceptions;

namespace Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error no manejado");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string message;

            switch (exception)
            {
                case InvalidAmountException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;

                case DuplicateUserDataException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = exception.Message;
                    break;

                case UnauthorizedException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = exception.Message;
                    break;

                case NotUserInTeamException:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = exception.Message;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Ocurrió un error inesperado";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = message,
                status = statusCode
            };

            return context.Response.WriteAsJsonAsync(response);
        }

    }

}
