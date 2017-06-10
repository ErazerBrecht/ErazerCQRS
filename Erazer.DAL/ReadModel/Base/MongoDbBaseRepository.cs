using MongoDB.Driver;

namespace Erazer.DAL.ReadModel.Base
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
