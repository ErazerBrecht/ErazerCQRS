using System.Threading.Tasks;
using Erazer.Domain.Ticket.Constants;
using Erazer.Read.Data.Ticket;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Erazer.Migrations.Seeding
{
    public static class StatusSeeder
    {
        public static async Task Seed(IMongoCollection<StatusDto> collection)
        {
            if (!await collection.AsQueryable().AnyAsync())
            {
                await Task.WhenAll
                (
                    collection.InsertOneAsync(new StatusDto { Id = StatusConstants.Backlog, Name = "Backlog" }),
                    collection.InsertOneAsync(new StatusDto { Id = StatusConstants.Open, Name = "Open" }),
                    collection.InsertOneAsync(new StatusDto { Id = StatusConstants.InProgress, Name = "In Progress" }),
                    collection.InsertOneAsync(new StatusDto { Id = StatusConstants.Done, Name = "Done" }),
                    collection.InsertOneAsync(new StatusDto { Id = StatusConstants.Closed, Name = "Closed" })
                );
            }
        }
    }
}
