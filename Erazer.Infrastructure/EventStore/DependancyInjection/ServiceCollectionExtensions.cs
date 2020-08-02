using System;
using System.Linq;
using System.Reflection;
using Erazer.Framework.Events;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ReadStore;
using Erazer.Syncing.Infrastructure;
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
        /// <param name="eventTypeAssemblies">Assemblies where there are 'EventTypes'</param>
        /// <returns></returns>
        public static EventStoreBuilder AddEventStore(this IServiceCollection services, IConfiguration configuration, params Type[] eventTypeAssemblies)
        {
            services.Configure<EventStoreSettings>(configuration);
            
            services.TryAddSingletonFactory<IStreamStore, EventStoreFactory>();
            services.TryAddSingleton<IEventStore, EventStore>();
            services.TryAddSingleton<IEventTypeMapping>(_ => new EventTypeMapping(eventTypeAssemblies?.Select(x => x.GetTypeInfo()?.Assembly).ToArray()));
            
            services.TryAddSingleton<ITelemetry, TelemetryLogging>();
            return new EventStoreBuilder(services);
        }

        public static EventStoreBuilder AddLiveSubscriber(this EventStoreBuilder builder)
        {
            builder.Collection.AddReadOnlyStore();
            builder.Collection.AddSingleton<ISubscription, LiveSubscription>();
            builder.Collection.AddSingleton<IHostedService, SubscriptionBackgroundService>();

            builder.Collection.AddSingleton(_ => builder.Collection);
            
            return builder;
        }
        
        public static EventStoreBuilder AddReSyncSubscriber(this EventStoreBuilder builder)
        {
            builder.Collection.AddReadOnlyStore();
            builder.Collection.AddSingleton<ISubscription, ReSyncSubscription>();
            builder.Collection.AddSingleton<IHostedService, SubscriptionBackgroundService>();

            builder.Collection.AddSingleton(_ => builder.Collection);

            return builder;
        }
        
        public static EventStoreBuilder AddComboSubscriber(this EventStoreBuilder builder)
        {
            builder.Collection.AddReadOnlyStore();
            builder.Collection.AddSingleton<ISubscription, ComboSubscription>();
            builder.Collection.AddSingleton<IHostedService, SubscriptionBackgroundService>();

            builder.Collection.AddSingleton(_ => builder.Collection);

            return builder;
        }
    }
}