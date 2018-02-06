using MongoDB.Driver;
using System.Threading.Tasks;

namespace Erazer.Infrastructure.MongoDb.Base
{
    public static class MongoDbExtensions
    {
        public static Task<IAsyncCursor<T>> FindAllAsync<T>(this IMongoCollection<T> collection)
        {
            return collection.FindAsync(_ => true);
        }
    }
}
