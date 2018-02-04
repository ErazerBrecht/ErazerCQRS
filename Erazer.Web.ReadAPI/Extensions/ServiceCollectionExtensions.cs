using Erazer.DAL.Events;
using Erazer.Framework.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Web.ReadAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void StartSubscriber<T>(this IServiceCollection collection) where T: AggregateRoot
        {
            collection.AddSingleton(typeof(ISubscription<>), typeof(EventStoreSubscription<>));

            // Build the intermediate service provider
            var sp = collection.BuildServiceProvider();
            var service = sp.GetService<ISubscription<T>>();

            service.Connect();
        }
    }
}
