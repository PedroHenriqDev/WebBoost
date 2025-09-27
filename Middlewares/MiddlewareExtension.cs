using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebBoost.Filters;

namespace WebBoost.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            return app;
        }

        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            services.AddSingleton<GlobalExceptionHandlerMiddleware>();
            return services;
        }
    }
}
