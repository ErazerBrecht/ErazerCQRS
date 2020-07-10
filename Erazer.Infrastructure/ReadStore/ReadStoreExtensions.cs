using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.ReadStore
{
    public static class ReadStoreExtensions
    {
        public static IServiceCollection AddReadOnlyStore(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDbQuery<>), typeof(DbQuery<>));
            return services;
        }
    }
}
