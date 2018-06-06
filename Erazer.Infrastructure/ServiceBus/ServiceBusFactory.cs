using System;
using Erazer.Framework.Factories;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erazer.Infrastructure.ServiceBus
{
    public interface IServiceBusFactory: IFactory<IBusControl>
    {
        IBusControl Configure(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null);
    }

    public class ServiceBusFactory: IServiceBusFactory
    {
        private readonly ILogger<ServiceBusFactory> _logger;
        private readonly ServiceBusSettings _settings;

        public ServiceBusFactory(ILogger<ServiceBusFactory> logger, IOptions<ServiceBusSettings> options)
        {
            _logger = logger;
            _settings = options.Value;

            _logger.LogInformation($"Building a MassTransit connection to a RabbitMQ Servicebus with connection string {_settings.ConnectionString}");
        }

        public IBusControl Build()
        {
            return Configure();
        }
        public IBusControl Configure(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(_settings.ConnectionString), hst =>
                {
                    hst.Username(_settings.UserName);
                    hst.Password(_settings.Password);
                });

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}
