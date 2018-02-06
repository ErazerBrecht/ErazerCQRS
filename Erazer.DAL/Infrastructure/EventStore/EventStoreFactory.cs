using System;
using Erazer.Framework.Factories;
using EventStore.ClientAPI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erazer.DAL.Infrastucture.EventStore
{
    public class EventStoreFactory : IFactory<IEventStoreConnection>
    {
        private readonly IOptions<EventStoreSettings> _options;
        private readonly ILogger<EventStoreFactory> _logger;

        public EventStoreFactory(IOptions<EventStoreSettings> options, ILogger<EventStoreFactory> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a 'GetEventStore' server");

            _logger.LogInformation($"Building a connection to a 'GetEventStore' server\n\t ConnectionString: {options.Value.ConnectionString}");
        }

        public IEventStoreConnection Build()
        {
            try
            {
                // TODO Use different settings on different env's
                var settings = ConnectionSettings.Create()
                                    .SetHeartbeatTimeout(TimeSpan.FromSeconds(5))
                                    .UseConsoleLogger();
                                    
                var connection = EventStoreConnection.Create(_options.Value.ConnectionString, settings);
                connection.ConnectAsync().Wait();

                _logger.LogInformation($"Created a succesful connection with the 'GetEventStore' server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t");
                return connection;
            }
            catch (Exception)
            {
                _logger.LogCritical($"Could NOT create a succesful connection with the 'GetEventStore' server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t");
                throw;
            }
        }
    }
}
