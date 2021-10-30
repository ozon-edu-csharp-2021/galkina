using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            LogRequest(context);
            await _next(context);
        }

        private void LogRequest(HttpContext context)
        {
            try
            {
                string route = context.Request.Path;
                
                var builder = new StringBuilder(Environment.NewLine);
                foreach (var header in context.Request.Headers)
                {
                    builder.AppendLine($"{header.Key}:{header.Value}");
                }

                _logger.LogInformation($"Request headers for route {route}");
                _logger.LogInformation(builder.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not log request headers");
            }
        }
    }
}