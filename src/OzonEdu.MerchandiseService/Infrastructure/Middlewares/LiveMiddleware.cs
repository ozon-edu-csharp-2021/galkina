using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class LiveMiddleware
    {
        public LiveMiddleware(RequestDelegate next)
        {
        }

        public async Task<int> InvokeAsync(HttpContext context)
        {
            return context.Response.StatusCode = 200;
        }
    }
}