using Erazer.Framework.Factories;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;
using System;

namespace Erazer.Infrastructure.MongoDb
{
    public class MongoDbFactory : IFactory<IMongoDatabase>
    {
        private readonly IOptions<MongoDbSettings> _options;
        private readonly ILogger<IMongoDatabase> _logger;
        private readonly TelemetryClient _telemetryClient;

        public MongoDbFactory(IOptions<MongoDbSettings> options, ILogger<IMongoDatabase> logger, TelemetryClient telemeteryClient)
        {
            _options = options;
            _logger = logger;
            _telemetryClient = telemeteryClient;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new MongoConfigurationException("Connection string is required when setting up a connection with a MongoDb server!");
            if (string.IsNullOrWhiteSpace(options.Value.Database))
                throw new MongoConfigurationException("Database name is required when setting up a connection with a MongoDb server!");

            _logger.LogInformation($"Building a connection to a mongo db server\n\t ConnectionString: {options.Value.ConnectionString}\n\t Database: {options.Value.Database}");
        }

        public IMongoDatabase Build()
        {
            var url = new Uri(_options.Value.ConnectionString);
            var client = new MongoClient(new MongoClientSettings()
            {
                Server = new MongoServerAddress(url.Host, url.Port),
                ClusterConfigurator = AddTelemeteryLogging()
            });

            var db = client.GetDatabase(_options.Value.Database);

            // Check if MongoDb connection is succesful created!
            try
            {
                db.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait();
                _logger.LogInformation($"Created a succesful connection with the mongo db server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t Database: {_options.Value.Database}");
            }
            catch
            {
                throw new MongoClientException($"Could NOT create a succesful connection with 'MongoDb' server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t");
            }
        
            return db;
        }

        private Action<ClusterBuilder> AddTelemeteryLogging()
        {
            return cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    if (ShouldCommandEventBeLogged(e.CommandName))
                        _telemetryClient.TrackTrace("MongoDB Command started - " + e.Command.ToJson());
                });
                cb.Subscribe<CommandSucceededEvent>(e =>
                {
                    if (ShouldCommandEventBeLogged(e.CommandName))
                        _telemetryClient.TrackDependency("MongoDB", "Command succeeded - " + e.CommandName, DateTime.Now, e.Duration, true);
                });
                cb.Subscribe<CommandFailedEvent>(e =>
                {
                    _telemetryClient.TrackDependency("MongoDB", $"Command failed - {e.CommandName} - {e.ToString()}", DateTime.Now.Subtract(e.Duration), e.Duration, false);
                });
            };
        }

        private bool ShouldCommandEventBeLogged(string commandName)
        {
            if (commandName != "isMaster" && commandName != "buildInfo" && commandName != "getLastError" && commandName != "ping")
                return true;
            return false;
        }
    }
}
