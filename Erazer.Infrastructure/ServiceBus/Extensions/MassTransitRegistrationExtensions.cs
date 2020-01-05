using System;
using System.Linq;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;

namespace Erazer.Infrastructure.ServiceBus.Extensions
{
    public static class MassTransitRegistrationExtensions
    {
        public static void AddConsumers(this IServiceCollectionConfigurator cfg, ServiceBusSettings config)
        {
            cfg.AddConsumers(config.EventConfig);
            cfg.AddConsumers(config.CommandConfig);
        }
        
        public static void AddEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IServiceProvider provider, ServiceBusSettings config)
        {
            cfg.AddEndpoint(provider, config.EventConfig);
            cfg.AddEndpoint(provider, config.CommandConfig);
        }

        private static void AddConsumers(this IServiceCollectionConfigurator cfg, IConsumerConfig config)
        {
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));
            if (config == null) return;

            foreach (var type in config.Listeners)
            {
                cfg.AddConsumer(type);
            }
        }

        private static void AddEndpoint(this IRabbitMqBusFactoryConfigurator cfg, IServiceProvider provider, IConsumerConfig consumerConfig)
        {
            if (consumerConfig == null) return;

            cfg.ReceiveEndpoint(consumerConfig.QueueName, e =>
            {
                e.PrefetchCount = consumerConfig.PrefetchCount ?? 32;
                e.Durable = consumerConfig.Durable;
                e.UseMessageRetry(x => x.Interval(5, 250));
                e.ConfigureConsumer(provider, consumerConfig.Listeners.ToArray());
            });
        }
    }
}
