using MongoDB.Bson.Serialization;

namespace Erazer.Infrastructure.MongoDb
{
    public abstract class MongoDbClassMap<T>
    {
        private static readonly object _syncLock = new object();

        protected MongoDbClassMap()
        {
            lock (_syncLock)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                    BsonClassMap.RegisterClassMap<T>(Map);
            }
        }

        public abstract void Map(BsonClassMap<T> cm);
    }
}