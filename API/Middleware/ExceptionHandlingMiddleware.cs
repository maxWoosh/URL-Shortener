using System.Net;
using System.Text.Json;

namespace API.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Передаем запрос дальше по конвейеру
                await _next(context);
            }

            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "An unhandled exception occurred while processing request {Path}", context.Request.Path);

                // Возвращаем ответ
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // определяем статус-код в зависимости от типа исключения
            var (statusCode, message) = exception switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
                _ => (HttpStatusCode.InternalServerError, "An internal error occurred. Please try again later.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = message,
                statusCode = (int)statusCode,
                timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
