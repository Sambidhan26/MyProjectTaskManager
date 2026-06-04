using TaskManager.API.Models.Common;

namespace TaskManager.API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new ErrorResponse
            {
                Success = false,
                Message = "An unexpected error occurred.",
                Details = exception.Message
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
