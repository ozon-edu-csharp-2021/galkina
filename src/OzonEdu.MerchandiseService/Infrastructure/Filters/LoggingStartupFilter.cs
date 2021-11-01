using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using OzonEdu.MerchandiseService.Infrastructure.Middlewares;

namespace OzonEdu.MerchandiseService.Infrastructure.Filters
{
    public class LoggingStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseWhen(context =>
                {
                    return context.Request.Path.StartsWithSegments("/api");
                }, a => {
                    a.UseMiddleware<ResponseLoggingMiddleware>();
                    a.UseMiddleware<RequestLoggingMiddleware>();
                });

                next(app);
            };
        }
    }
}