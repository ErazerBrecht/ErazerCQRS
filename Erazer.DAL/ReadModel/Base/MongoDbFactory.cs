﻿using Erazer.Framework.Factories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;

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
    }
}
