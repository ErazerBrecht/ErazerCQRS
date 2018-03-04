using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;
using EasyNetQ;

namespace Erazer.Infrastructure.ServiceBus
{
    public class BusFactory : IFactory<IBus>
    {
        private readonly ILogger<IBus> _logger;

        public BusFactory(ILogger<IBus> logger)
        {
            _logger = logger;

            _logger.LogInformation($"Building a connection to a RabbitMQ Servicebus");
        }

        public IBus Build()
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            // TODO Check if connection was actually succeeded!
            _logger.LogInformation($"Created a succesful connection with the RabbitMQ Servicebus");

            return bus;
        }
    }
}
