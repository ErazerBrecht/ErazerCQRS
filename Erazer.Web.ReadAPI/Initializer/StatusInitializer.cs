using Erazer.Domain.Constants;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using System.Threading.Tasks;

namespace Erazer.Web.ReadAPI.Initializer
{
    public static class StatusInitializer
    {
        public static async Task Initialize(IStatusQueryRepository repository)
        {
            if(!await repository.Any())
            {
                await repository.Add(new StatusDto() { Id= StatusConstants.Backlog, Name= "Backlog" });
                await repository.Add(new StatusDto() { Id= StatusConstants.Open, Name= "Open" });
                await repository.Add(new StatusDto() { Id= StatusConstants.InProgress, Name= "In Progress" });
                await repository.Add(new StatusDto() { Id= StatusConstants.Done, Name= "Done" });
                await repository.Add(new StatusDto() { Id= StatusConstants.Closed, Name= "Closed" });
            }
        }
    }
}
