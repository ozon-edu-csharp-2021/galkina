﻿using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public class ProducerBuilderWrapper : IProducerBuilderWrapper
    {
        public IProducer<string, string> Producer { get; set; }
        public string EmployeeNotificationTopic { get; set; }

        public ProducerBuilderWrapper(IOptions<KafkaConfiguration> configuration)
        {
            var configValue = configuration.Value;
            if (configValue is null)
                throw new ApplicationException("Configuration for kafka server was not specified");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configValue.BootstrapServers
            };

            Producer = new ProducerBuilder<string, string>(producerConfig).Build();
            EmployeeNotificationTopic = configValue.Topic;
        }
    }
}