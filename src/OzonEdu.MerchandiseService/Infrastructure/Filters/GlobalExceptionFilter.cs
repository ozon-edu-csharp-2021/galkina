using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OzonEdu.MerchandiseService.Infrastructure.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        
        public override void OnException(ExceptionContext context)
        {
            string exceptionType = context.Exception.GetType().FullName;
            string message = context.Exception.Message;
            string stackTrace = context.Exception.StackTrace;
            
            var exceptionObject = new
            {
                ExceptionType = exceptionType,
                Message = message
            };

            var jsonResult = new JsonResult(exceptionObject)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            
            _logger.LogError($"Exception Error: {exceptionType}\n" +
                             $"Message: {message}\n" +
                             $"StackTrace: {stackTrace}");
            
            context.Result = jsonResult;
        }
    }
}