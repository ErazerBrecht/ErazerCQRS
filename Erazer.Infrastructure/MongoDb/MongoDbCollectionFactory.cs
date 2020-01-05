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
    public class MongoDbCollectionFactory<T> : IFactory<IMongoCollection<T>>
    {
        private readonly IMongoDatabase _database;

        public MongoDbCollectionFactory(IMongoDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IMongoCollection<T> Build()
        {
            var collectionName = CollectionNameMapping.RetrieveCollectionName(typeof(T));
            return _database.GetCollection<T>(collectionName);
        }
    }
}