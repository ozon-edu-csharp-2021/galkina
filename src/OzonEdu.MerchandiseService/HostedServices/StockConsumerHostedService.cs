using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseService.HostedServices
{
    public class StockConsumerHostedService : BackgroundService
    {
        private readonly KafkaConfiguration _config;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<StockConsumerHostedService> _logger;

        protected string Topic { get; set; } = "stock_replenished_event";
        
        public StockConsumerHostedService(
            IOptions<KafkaConfiguration> config,
            IServiceScopeFactory scopeFactory,
            ILogger<StockConsumerHostedService> logger)
        {
            _config = config.Value;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _config.GroupId,
                BootstrapServers = _config.BootstrapServers,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                c.Subscribe(Topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            try
                            {
                                await Task.Yield();
                                Console.WriteLine("This is background servise");
                                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                                var cr = c.Consume(stoppingToken);
                                if (cr != null)
                                {
                                    Console.WriteLine("This is message");
                                    var message = JsonSerializer.Deserialize<StockReplenishedEvent>(cr.Message.Value);
                                    await mediator.Send(new GiveUnreleasedMerchPacksCommand(), stoppingToken);
                                }
                            }
                            catch (KafkaException ex)
                            {
                                _logger.LogError($"Error while get consume. Message {ex.Message}");
                            }
                        }
                    }
                }
                finally
                {
                    c.Commit();
                    c.Close();
                }
            }
        }
    }
}