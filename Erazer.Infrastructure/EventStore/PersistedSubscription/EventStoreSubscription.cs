using AutoMapper;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public class EventStoreSubscription<T> : ISubscription<T>, IDisposable where T : AggregateRoot
    {
        private const string _groupName = "erazercqrs";

        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IServiceProvider _provider;
        private readonly TelemetryClient _telemetryClient;

        private EventStorePersistentSubscriptionBase EventStorePersistentSubscriptionBase { get; set; }

        public EventStoreSubscription(IEventStoreConnection eventStoreConnection, TelemetryClient telemeteryClient, IServiceProvider provider)
        {
            _eventStoreConnection = eventStoreConnection;
            _telemetryClient = telemeteryClient;
            _provider = provider;
        }


        private static void CreateSubscription(IEventStoreConnection conn)
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();
        }


        public async Task Connect()
        {
            var streamName = $"$ce-{typeof(T).Name}";

            EventStorePersistentSubscriptionBase = await _eventStoreConnection.ConnectToPersistentSubscriptionAsync(
                   streamName,
                   _groupName,
                   EventAppeared,
                   SubscriptionDropped
            );
        }

        private void SubscriptionDropped(EventStorePersistentSubscriptionBase subscription, SubscriptionDropReason reason, Exception ex)
        {
            _telemetryClient.TrackEvent("SubscriptionDropped", new Dictionary<string, string> { { "Reason", reason.ToString() } });

            if (ex != null)
                _telemetryClient.TrackException(ex);

            Connect().Wait();
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore (read model)", new Dictionary<string, string> {
                { "Type", resolvedEvent.Event.EventType },
                { "EventNumber", resolvedEvent.Event.EventNumber.ToString() },
                { "Created (Epoch)", resolvedEvent.Event.CreatedEpoch.ToString() }
            });


            using (var scope = _provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var @event = mapper.Map<IDomainEvent>(resolvedEvent);
                await mediator.Publish(@event);
            }
        }


        public void Dispose()
        {
            EventStorePersistentSubscriptionBase.Stop(TimeSpan.FromSeconds(15));
        }
    }
}
