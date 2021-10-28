using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<ResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            LogResponse(context);
            await _next(context);
        }

        private void LogResponse(HttpContext context)
        {
            try
            {
                string route = context.Request.Path;

                var builder = new StringBuilder(Environment.NewLine);
                foreach (var header in context.Response.Headers)
                {
                    builder.AppendLine($"{header.Key}:{header.Value}");
                }
                
                _logger.LogInformation($"Response headers for route {route}");
                _logger.LogInformation(builder.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log response headers");
            }
        }
    }
}