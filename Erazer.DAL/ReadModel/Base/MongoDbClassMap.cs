using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Erazer.DAL.ReadModel.Base
{
    public abstract class MongoDbClassMap<T>
    {
        protected MongoDbClassMap()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                BsonClassMap.RegisterClassMap<T>(Map);
        }

        public abstract void Map(BsonClassMap<T> cm);
    }
}
