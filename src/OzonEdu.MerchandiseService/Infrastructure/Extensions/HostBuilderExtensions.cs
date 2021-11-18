using System;
using System.IO;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using OzonEdu.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Infrastructure.Interceptors;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation;

namespace OzonEdu.MerchandiseService.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            return builder
                .ConfigureVersionEndpoint()
                .ConfigureHttp()
                .ConfigureGrpc()
                .ConfigureSwagger()
                .ConfigureDataLayer();
        }

        private static IHostBuilder ConfigureVersionEndpoint(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter, LoggingStartupFilter>();
                services.AddSingleton<IStartupFilter, TerminalStartupFilter>();
            });

            return builder;
        }
        
        private static IHostBuilder ConfigureHttp(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
            });

            return builder;
        }
        
        private static IHostBuilder ConfigureGrpc(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());
            });

            return builder;
        }
        
        private static IHostBuilder ConfigureSwagger(this IHostBuilder builder)
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
            });
            
            return builder;
        }
        
        private static IHostBuilder ConfigureDataLayer(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                AddMediator(services);
                AddDatabaseComponents(services);
                AddRepositories(services);
            });

            return builder;
        }
        
        private static void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        private static void AddDatabaseComponents(IServiceCollection services)
        {
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped(typeof(IMerchPackRepository), typeof(MerchPackRepository));
        }
    }
}