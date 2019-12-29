using Erazer.Domain.Data.Repositories;

namespace Erazer.Web.ReadAPI.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void Seed(this IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            {
                var services = scope.ServiceProvider;
                Task.WaitAll(
                    CollectionSeeder.Seed(services.GetRequiredService<IMongoDatabase>()),
                    StatusSeeder.Seed(services.GetRequiredService<IStatusQueryRepository>()),
                    PrioritySeeder.Seed(services.GetRequiredService<IPriorityQueryRepository>())
                );
            }
        }
    }
}
