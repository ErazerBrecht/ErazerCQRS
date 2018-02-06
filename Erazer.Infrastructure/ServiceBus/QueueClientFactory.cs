using System;
using Erazer.Framework.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erazer.Infrastructure.ServiceBus
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
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a Azure Servicebus");
            if (string.IsNullOrWhiteSpace(options.Value.QueueName))
                throw new ArgumentNullException(options.Value.QueueName, "Queue name is required when setting up a connection with a Azure Servicebus");

            _logger.LogInformation($"Building a connection to a Azure Servicebus\n\t ConnectionString: {options.Value.ConnectionString} \n\t QueueName: {options.Value.QueueName}");
        }

        public IQueueClient Build()
        {
            var queueClient = new QueueClient(_options.Value.ConnectionString, _options.Value.QueueName);

            // TODO Check if connection was actually succeeded!
            _logger.LogInformation($"Created a succesful connection with the Azure Servicebus\n\t ConnectionString: {_options.Value.ConnectionString}\n\t QueueName: {_options.Value.QueueName}");

            return queueClient;
        }
    }
}
