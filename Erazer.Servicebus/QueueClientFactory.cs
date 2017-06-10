using System;
using Erazer.Framework.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erazer.Servicebus
{
    public class QueueClientFactory : IFactory<IQueueClient>
    {
        private readonly IOptions<AzureServiceBusSettings> _options;
        private readonly ILogger<IQueueClient> _logger;

        public QueueClientFactory(IOptions<AzureServiceBusSettings> options, ILogger<IQueueClient> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a PostgresSQL server for using 'Marten'");

            _logger.LogInformation($"Building a connection to a PostgreSQL server\n\t ConnectionString: {options.Value.ConnectionString}");
        }

        public IQueueClient Build()
        {
            var queueClient = new QueueClient(_options.Value.ConnectionString, _options.Value.QueueName);

            // TODO Check if connection was actually succeeded!
            _logger.LogInformation($"Created a succesful connection with the Azure Servicebus\n\t ConnectionString: {_options.Value.ConnectionString}\n\t");

            return queueClient;
        }
    }
}
