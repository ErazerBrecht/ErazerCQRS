using System;
using Erazer.Framework.Factories;
using Erazer.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Events;

namespace Erazer.Infrastructure.MongoDb
{
    public class MongoDbFactory : IFactory<IMongoDatabase>
    {
        private readonly IOptions<MongoDbSettings> _options;
        private readonly ILogger<IMongoDatabase> _logger;
        private readonly ITelemetry _telemetryClient;

        public MongoDbFactory(IOptions<MongoDbSettings> options, ILogger<IMongoDatabase> logger, ITelemetry telemetryClient)
        {
            _options = options;
            _logger = logger;
            _telemetryClient = telemetryClient;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new MongoConfigurationException("Connection string is required when setting up a connection with a MongoDb server!");
            if (string.IsNullOrWhiteSpace(options.Value.Database))
                throw new MongoConfigurationException("Database name is required when setting up a connection with a MongoDb server!");

            _logger.LogInformation($"Building a connection to a mongo db server\n\t ConnectionString: {options.Value.ConnectionString}\n\t Database: {options.Value.Database}");
        }

        public IMongoDatabase Build()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(_options.Value.ConnectionString));
            settings.ClusterConfigurator = AddTelemetryLogging();

            var client = new MongoClient(settings);
            var db = client.GetDatabase(_options.Value.Database);

            // Check if MongoDb connection is successful created!
            try
            {
                db.RunCommandAsync((Command<BsonDocument>) "{ping:1}").Wait();
                _logger.LogInformation($"Created a successful connection with the 'MongoDb' server");
            }
            catch
            {
                var ex = new MongoClientException($"Could NOT create a successful connection with 'MongoDb'");
                _logger.LogCritical(ex, $"Could NOT create a successful connection with the 'MongoDb'");
                throw ex;
            }

            return db;
        }

        private Action<ClusterBuilder> AddTelemetryLogging()
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
                        _telemetryClient.TrackDependency("DB", "MongoDB", $"Command succeeded {e.CommandName}", DateTime.Now, e.Duration, true);
                });
                cb.Subscribe<CommandFailedEvent>(e =>
                {
                    _telemetryClient.TrackDependency("DB", "MongoDB", $"Command failed {e.CommandName} - {e.ToString()}", DateTime.Now, e.Duration, false);
                });
            };
        }

        private static bool ShouldCommandEventBeLogged(string commandName)
        {
            return commandName != "isMaster" && commandName != "buildInfo" && commandName != "getLastError" && commandName != "ping";
        }
    }
}
