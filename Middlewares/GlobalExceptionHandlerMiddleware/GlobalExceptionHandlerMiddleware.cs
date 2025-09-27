using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebBoost.Filters
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                _logger.LogCritical($"Exception: {ex.InnerException} \n Message: {ex.Message} \n Stack Trace: {ex.StackTrace} \n Source: {ex.Source}");
                await context.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
            }
        }
    }
}
