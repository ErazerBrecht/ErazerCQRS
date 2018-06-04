using System.Threading.Tasks;
using Erazer.Domain.Constants;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.Repositories;

namespace Erazer.Infrastructure.ReadStore.Seeding
{
    public static class StatusSeeder
    {
        public static async Task Seed(IStatusQueryRepository repository)
        {
            if (!await repository.Any())
            {
                await Task.WhenAll(
                    repository.Add(new StatusDto() { Id = StatusConstants.Backlog, Name = "Backlog" }),
                    repository.Add(new StatusDto() { Id = StatusConstants.Open, Name = "Open" }),
                    repository.Add(new StatusDto() { Id = StatusConstants.InProgress, Name = "In Progress" }),
                    repository.Add(new StatusDto() { Id = StatusConstants.Done, Name = "Done" }),
                    repository.Add(new StatusDto() { Id = StatusConstants.Closed, Name = "Closed" })
                );
            }
        }
    }
}
