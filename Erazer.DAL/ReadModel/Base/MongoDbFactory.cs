using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.Base
{
    public class MongoDbFactory : IFactory<IMongoDatabase>
    {
        private readonly IOptions<MongoDbSettings> _options;
        private readonly ILogger<IMongoDatabase> _logger;

        public MongoDbFactory(IOptions<MongoDbSettings> options, ILogger<IMongoDatabase> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new MongoConfigurationException("Connection string is required when setting up a connection with a MongoDb server!");
            if (string.IsNullOrWhiteSpace(options.Value.Database))
                throw new MongoConfigurationException("Database name is required when setting up a connection with a MongoDb server!");

            _logger.LogInformation($"Building a connection to a mongo db server\n\t ConnectionString: {options.Value.ConnectionString}\n\t Database: {options.Value.Database}");
        }

        public IMongoDatabase Build()
        {
            var client = new MongoClient(_options.Value.ConnectionString);
            var db = client.GetDatabase(_options.Value.Database);

            _logger.LogInformation($"Created a succesful connection with the mongo db server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t Database: {_options.Value.Database}");

            return db;
        }
    }
}
