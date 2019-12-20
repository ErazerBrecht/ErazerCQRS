using Erazer.Infrastructure.MongoDb.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.ReadStore.ClassMaps;
using Erazer.Infrastructure.ReadStore.Repositories;
using Microsoft.Extensions.Hosting;

namespace Erazer.Web.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSubscriber(this IServiceCollection collection)
        {
            collection.AddSingleton<IEventTypeMapping, EventTypeMapping>();
            collection.AddSingleton<IPositionRepository, PositionRepository>();

            collection.AddSingleton<ISubscription, Subscription>();
            collection.AddSingleton<IHostedService, SubscriptionBackgroundService>();
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
