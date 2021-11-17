using System;
using System.IO;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OzonEdu.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Infrastructure.Interceptors;
using OzonEdu.MerchandiseService.Infrastructure.Stub;

namespace OzonEdu.MerchandiseService.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                 services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
                 services.AddSwaggerGen(options =>
                 {
                     string serviceName = Assembly.GetExecutingAssembly().GetName().Name ?? "no name";
                     string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "no version";
                     
                     options.SwaggerDoc("v1", new OpenApiInfo {Title = serviceName, Version = version});
                 
                     options.CustomSchemaIds(x => x.FullName);
                 
                     string xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                     string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                     options.IncludeXmlComments(xmlFilePath);
                 });

                 services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
                 services.AddSingleton<IStartupFilter, LoggingStartupFilter>();
                 services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
                
                 services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());

                 services.AddScoped<IUnitOfWork, UnitOfWorkStub>();
                 services.AddScoped(typeof(IMerchPackRepository), typeof(MerchPackRepositoryStub));
                 
                 services.AddMediatR(Assembly.GetExecutingAssembly());
            });
            return builder;
        }
    }
}