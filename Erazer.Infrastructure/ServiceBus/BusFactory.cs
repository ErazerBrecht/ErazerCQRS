using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;
using EasyNetQ;
using Microsoft.Extensions.Options;

namespace Erazer.Infrastructure.ServiceBus
{
    public class BusFactory : IFactory<IBus>
    {
        private readonly ILogger<IBus> _logger;
        private readonly ServiceBusSettings _settings;

        public BusFactory(ILogger<IBus> logger, IOptions<ServiceBusSettings> options)
        {
            _logger = logger;
            _settings = options.Value;

            _logger.LogInformation($"Building a connection to a RabbitMQ Servicebus with connection string {_settings.ConnectionString}");
        }

        public IBus Build()
        {
            var bus = RabbitHutch.CreateBus(_settings.ConnectionString);

            // TODO Check if connection was actually succeeded!
            _logger.LogInformation($"Created a succesful connection with the RabbitMQ Servicebus");

            return bus;
        }
    }
}
