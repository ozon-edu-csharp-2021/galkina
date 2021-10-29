using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            LogRequest(context);
            await _next.Invoke(context);
            LogResponse(context);
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