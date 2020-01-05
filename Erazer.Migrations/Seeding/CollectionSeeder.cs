using System.Linq;
using System.Threading.Tasks;
using Erazer.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace Erazer.Migrations.Seeding
{
    public static class CollectionSeeder
    {
        public static async Task Seed(IMongoDatabase db, CollectionNameDictionary neededCollections)
        {
            var collections = await (await db.ListCollectionsAsync()).ToListAsync();
            var collectionNames = collections.Select(y => y.GetValue("name").AsString).ToList();
            
            foreach (var collectionName in neededCollections.Values.Distinct().Where(collectionName => !collectionNames.Contains(collectionName)))
            {
                await db.CreateCollectionAsync(collectionName);
            }
        }
    }
}
