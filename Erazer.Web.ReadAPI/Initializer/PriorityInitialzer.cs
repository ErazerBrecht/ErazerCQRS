using Erazer.Domain.Constants;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using System.Threading.Tasks;

namespace Erazer.Web.ReadAPI.Initializer
{
    public static class PriorityInitialzer
    {
        public static async Task Initialize(IPriorityQueryRepository repository)
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
