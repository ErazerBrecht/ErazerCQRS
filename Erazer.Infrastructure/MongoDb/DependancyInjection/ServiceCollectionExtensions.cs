using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Erazer.Framework.DTO;
using Erazer.Framework.Events;
using Erazer.Framework.Factories;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.EventStore.MongoDb;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.ReadStore.ClassMaps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using SqlStreamStore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoDbDependencyInjection
    {
        /// <summary>
        /// Add 'MongoDb' related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">Key value representation of MongoDb settings</param>
        /// <param name="dtoBuilderAction">Optional settings for DTOs (document) like collection name, ...</param>
        /// <returns></returns>
        public static IServiceCollection AddMongo(this IServiceCollection services,
            IConfiguration configuration, Action<MongoDbDtoBuilder> dtoBuilderAction = null)
        {
            var assembly = new List<Assembly> { Assembly.GetCallingAssembly() };

            if (dtoBuilderAction != null)
            {
                var dtoBuilder = new MongoDbDtoBuilder();
                dtoBuilderAction(dtoBuilder);
                assembly = dtoBuilder.Assemblies ?? assembly;
            }

            services.Configure<MongoDbSettings>(configuration);
            services.AddMongoInternal(assembly);

            return services;
        }

        private static IServiceCollection AddMongoInternal(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.TryAddSingletonFactory<IMongoDatabase, MongoDbFactory>();

            services.AddMongoCollections(assemblies);
            services.AddMongoDbClassMaps();

            services.TryAddSingleton<ITelemetry, TelemetryLogging>();
            return services;
        }

        private static void AddMongoCollections(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton(typeof(MongoDbCollectionFactory<>));

            assemblies ??= new List<Assembly> {Assembly.GetExecutingAssembly()};

            var collectionTypes = assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(IDto)))
                .Distinct()
                .ToList();

            var collectionNames = new CollectionNameDictionary();
            
            foreach (var collectionType in collectionTypes)
            {
                var type = typeof(IMongoCollection<>).MakeGenericType(collectionType);
                var factoryType = typeof(MongoDbCollectionFactory<>).MakeGenericType(collectionType);

                collectionNames.Add(collectionType, CollectionNameMapping.RetrieveCollectionName(collectionType));
                
                services.AddSingleton(type, (x) =>
                {
                    var factory = x.GetRequiredService(factoryType);
                    dynamic dFactory = factory;
                    var implementation = dFactory.Build();
                    return implementation;
                });
            }

            services.AddSingleton(collectionNames);
        }

        private static void AddMongoDbClassMaps(this IServiceCollection services)
        {
            // TODO CHECK IF ALREADY REGISTERED!!!

            // This will only work if every class map is in the same assembly!!!
            var assembly = typeof(TicketClassMap).GetTypeInfo().Assembly;

            // Base class type
            var type = typeof(MongoDbClassMap<>);

            // Get all types that have MongoDbClassMap as their base class
            var classMaps = assembly.GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface &&
                            t.GetTypeInfo().BaseType != null && t.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType)
                .Where(t => t.GetTypeInfo().BaseType.GetGenericTypeDefinition() == type);

            // Create new instance of every class that has the 'MongoDbClass' as base
            foreach (var classMap in classMaps)
                Activator.CreateInstance(classMap);
        }
    }
}