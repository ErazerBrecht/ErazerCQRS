using Erazer.Domain.Constants;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using System.Threading.Tasks;

namespace Erazer.DAL.ReadModel.Seeding
{
    public static class StatusSeeder
    {
        public static async Task Seed(IStatusQueryRepository repository)
        {
            if (!await repository.Any())
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
