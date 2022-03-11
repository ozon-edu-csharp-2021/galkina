using System;
using System.IO;
using System.Net;
using System.Reflection;
using Grpc.Net.Client;
using OzonEdu.StockApi.Grpc;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using OzonEdu.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.HostedServices;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;
using OzonEdu.MerchandiseService.Infrastructure.Filters;
using OzonEdu.MerchandiseService.Infrastructure.Interceptors;
using OzonEdu.MerchandiseService.Infrastructure.MessageBroker;
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
                AddRepositories(services);
            });
            
            return builder;
        }
        
        private static void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddDatabaseComponents(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConnectionOptions>(configuration.GetSection(nameof(DatabaseConnectionOptions)));
            
            services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IChangeTracker, ChangeTracker>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddScoped(typeof(IMerchPackRepository), typeof(MerchPackRepository));
        }
        
        public static IHostBuilder ConfigurePorts(this IHostBuilder builder)
        {
            var httpPortEnv = Environment.GetEnvironmentVariable("HTTP_PORT");
            if (!int.TryParse(httpPortEnv, out var httpPort))
            {
                httpPort = 5000;
            }

            var grpcPortEnv = Environment.GetEnvironmentVariable("GRPC_PORT");
            if (!int.TryParse(grpcPortEnv, out var grpcPort))
            {
                grpcPort = 5002;
            }
            
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                httpPort = 90;
                grpcPort = 92;
            }
            
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureKestrel(
                    options =>
                    {
                        Listen(options, httpPort, HttpProtocols.Http1);
                        Listen(options, grpcPort, HttpProtocols.Http2);
                    });
            });
            return builder;
        }
        
        static void Listen(KestrelServerOptions kestrelServerOptions, int? port, HttpProtocols protocols)
        {
            if (port == null)
                return;

            var address = IPAddress.Any;

            kestrelServerOptions.Listen(address, port.Value, listenOptions => { listenOptions.Protocols = protocols; });
        }
        
        public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            services.AddSingleton<IProducerBuilderWrapper, ProducerBuilderWrapper>();

            return services;
        }
        
        public static IServiceCollection AddStockGrpcServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionAddress = configuration.GetSection(nameof(StockApiGrpcServiceConfiguration))
                .Get<StockApiGrpcServiceConfiguration>().ServerAddress;
            if(string.IsNullOrWhiteSpace(connectionAddress))
                connectionAddress = configuration
                    .Get<StockApiGrpcServiceConfiguration>()
                    .ServerAddress;

            services.AddScoped<StockApiGrpc.StockApiGrpcClient>(opt =>
            {
                var channel = GrpcChannel.ForAddress(connectionAddress);
                return new StockApiGrpc.StockApiGrpcClient(channel);
            });

            return services;
        }
        
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<StockConsumerHostedService>();

            return services;
        }
        
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KafkaConfiguration>(configuration);
            
            return services;
        }
    }
}