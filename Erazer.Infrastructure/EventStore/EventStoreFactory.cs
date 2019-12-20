using System;
using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlStreamStore;

namespace Erazer.Infrastructure.EventStore
{
    public class EventStoreFactory : IFactory<IStreamStore>
    {
        private readonly IOptions<EventStoreSettings> _options;
        private readonly ILogger<EventStoreFactory> _logger;

        public EventStoreFactory(IOptions<EventStoreSettings> options, ILogger<EventStoreFactory> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a 'GetEventStore' server");

            _logger.LogInformation($"Building a connection to the 'EventStore'");
        }

        public IStreamStore Build()
        {
            var settings = new PostgresStreamStoreSettings(_options.Value.ConnectionString); 
            return new PostgresStreamStore(settings);
        }
    }
}
