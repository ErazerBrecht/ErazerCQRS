using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Infrastructure.EventStore.PersistedSubscription;
using Erazer.Infrastructure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Web.Shared.Extensions
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

        public static void StartReciever(this IServiceCollection collection)
        {
            collection.AddSingleton<IEventReciever, EventReciever>();

            // Build the intermediate service provider
            var sp = collection.BuildServiceProvider();
            var service = sp.GetService<IEventReciever>();

            service.RegisterEventReciever();
        }
    }
}
