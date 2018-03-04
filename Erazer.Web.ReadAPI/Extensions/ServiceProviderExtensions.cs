using Erazer.Domain.Data.Repositories;
using Erazer.Infrastructure.MongoDb.Seeding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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
                    StatusSeeder.Seed(services.GetRequiredService<IStatusQueryRepository>()),
                    PrioritySeeder.Seed(services.GetRequiredService<IPriorityQueryRepository>())
                );
            }
        }
    }
}
