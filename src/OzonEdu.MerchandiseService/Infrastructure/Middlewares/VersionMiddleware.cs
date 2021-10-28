using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OzonEdu.MerchandiseService.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        public VersionMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
            string serviceName = Assembly.GetExecutingAssembly().GetName().Name ?? "no name";
            string versionAndServiceName = string.Format($"version: {version}, serviceName: {serviceName}");
            await context.Response.WriteAsync(versionAndServiceName);
        }
    }
}