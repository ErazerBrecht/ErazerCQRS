using System.Threading.Tasks;
using Erazer.Domain.Ticket.Constants;
using Erazer.Read.Data.Ticket;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Erazer.Migrations.Seeding
{
    public static class PrioritySeeder
    {
        public static async Task Seed(IMongoCollection<PriorityDto> collection)
        {
            if (!await collection.AsQueryable().AnyAsync())
            {
                await Task.WhenAll
                (
                    collection.InsertOneAsync(new PriorityDto {Id = PriorityConstants.Lowest, Name = "Lowest"}),
                    collection.InsertOneAsync(new PriorityDto {Id = PriorityConstants.Low, Name = "Low"}),
                    collection.InsertOneAsync(new PriorityDto {Id = PriorityConstants.Medium, Name = "Medium"}),
                    collection.InsertOneAsync(new PriorityDto {Id = PriorityConstants.High, Name = "High"}),
                    collection.InsertOneAsync(new PriorityDto {Id = PriorityConstants.Highest, Name = "Highest"})
                );
            }
        }
    }
}
