using MongoDB.Driver;

namespace Erazer.Infrastructure.MongoDb.Base
{
    public abstract class MongoDbBaseRepository
    {
        protected readonly IMongoDatabase Database;

        public MongoDbBaseRepository(IMongoDatabase database)
        {
            Database = database;
        }
    }
}
