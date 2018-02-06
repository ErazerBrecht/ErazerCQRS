using Erazer.Domain.Constants;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.Repositories;
using System.Threading.Tasks;

namespace Erazer.Infrastructure.MongoDb.Seeding
{
    public static class PrioritySeeder
    {
        public static async Task Seed(IPriorityQueryRepository repository)
        {
            if (!await repository.Any())
            {
                await repository.Add(new PriorityDto() { Id= PriorityConstants.Lowest, Name= "Lowest" });
                await repository.Add(new PriorityDto() { Id= PriorityConstants.Low, Name= "Low" });
                await repository.Add(new PriorityDto() { Id= PriorityConstants.Medium, Name= "Medium" });
                await repository.Add(new PriorityDto() { Id= PriorityConstants.High, Name= "High" });
                await repository.Add(new PriorityDto() { Id= PriorityConstants.Highest, Name= "Highest" });
            }
        }
    }
}
