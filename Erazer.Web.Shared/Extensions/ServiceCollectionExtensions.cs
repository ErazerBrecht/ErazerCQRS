using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using Erazer.Infrastructure.EventStore.PersistedSubscription;
using Erazer.Infrastructure.MongoDb.Base;
using Erazer.Infrastructure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Erazer.Infrastructure.ReadStore.ClassMaps;

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

        public static void AddMongoDbClassMaps(this IServiceCollection services)
        {
            // This will only work if every class map is in the same assembly!
            var assembly = typeof(TicketClassMap).GetTypeInfo().Assembly;

            // Base class type
            var type = typeof(MongoDbClassMap<>);

            // Get all types that have MongoDbClassMap as their base class
            var classMaps = assembly.GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface && t.GetTypeInfo().BaseType != null && t.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType)
                .Where(t => t.GetTypeInfo().BaseType.GetGenericTypeDefinition() == type);

            // Create new instance of every class that has the 'MongoDbClass' as base
            foreach (var classMap in classMaps)
                Activator.CreateInstance(classMap);
        }

    }
}
