using System;
using System.Linq;
using System.Reflection;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.ReadStore.ClassMaps;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.SeedWork
{
    public static class ServiceCollectionExtensions
    {
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
