using System;
using Erazer.Framework.Events;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.EventStore.MongoDb;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SqlStreamStore;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class EventStoreDependencyInjection
    {
        /// <summary>
        /// Add 'EventStore' related stuff to IoC
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="configuration">ConfigurationRoot to be able to configure the eventstore</param>
        /// <returns></returns>
        public static EventStoreBuilder AddEventStore(this IServiceCollection services,  IConfiguration configuration)
        {
            services.Configure<EventStoreSettings>(configuration);
            
            services.TryAddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.TryAddSingleton<IEventStore, EventStore>();
            services.TryAddSingleton<IEventTypeMapping, EventTypeMapping>();
            
            services.TryAddSingleton<ITelemetry, TelemetryLogging>();
            return new EventStoreBuilder(services);
        }

        public static EventStoreBuilder AddSubscriber(this EventStoreBuilder builder)
        {
            builder.Collection.AddSingleton<IPositionStore, PositionMongoDbStore>();

            builder.Collection.AddSingleton<ISubscription, Subscription>();
            builder.Collection.AddSingleton<IHostedService, SubscriptionBackgroundService>();

            return builder;
        }

       
    }
}