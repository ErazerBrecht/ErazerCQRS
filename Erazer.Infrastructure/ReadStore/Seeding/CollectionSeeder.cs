using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore.Seeding
{
    public static class CollectionSeeder
    {
        public static async Task Seed(IMongoDatabase db)
        {
            var collections = await (await db.ListCollectionsAsync()).ToListAsync();
            var collectionNames = collections.Select(y => y.GetValue("name").AsString).ToList();

            if (!collectionNames.Contains("Position"))
                await db.CreateCollectionAsync("Position");

            if (!collectionNames.Contains("Tickets"))
                await db.CreateCollectionAsync("Tickets");

            if (!collectionNames.Contains("TicketEvents"))
                await db.CreateCollectionAsync("TicketEvents");

            if (!collectionNames.Contains("Priorities"))
                await db.CreateCollectionAsync("Priorities");

            if (!collectionNames.Contains("Statuses"))
                await db.CreateCollectionAsync("Statuses");
        }
    }
}
