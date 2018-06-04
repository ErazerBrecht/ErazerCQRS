using AutoMapper;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Framework.FrontEnd;
using Erazer.Infrastructure.Websockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public class EventStoreSubscription<T> : ISubscription<T>, IDisposable where T : AggregateRoot
    {
        private const string _groupName = "erazercqrs";

        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly TelemetryClient _telemetryClient;
        private readonly IServiceScopeFactory _scopeFactory;

        private EventStorePersistentSubscriptionBase EventStorePersistentSubscriptionBase { get; set; }

        public EventStoreSubscription(IEventStoreConnection eventStoreConnection, IMapper mapper, IMediator mediator, TelemetryClient telemeteryClient, IServiceScopeFactory scopeFactory)
        {
            _eventStoreConnection = eventStoreConnection;
            _mapper = mapper;
            _mediator = mediator;
            _telemetryClient = telemeteryClient;
            _scopeFactory = scopeFactory;
        }


        private static void CreateSubscription(IEventStoreConnection conn)
        {
            PersistentSubscriptionSettings settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();
        }


        public void Connect()
        {
            var streamName = $"$ce-{typeof(T).Name}";

            EventStorePersistentSubscriptionBase = _eventStoreConnection.ConnectToPersistentSubscription(
                   streamName,
                   _groupName,
                   EventAppeared,
                   SubscriptionDropped
            );
        }

        private void SubscriptionDropped(EventStorePersistentSubscriptionBase subscription, SubscriptionDropReason reason, Exception ex)
        {
            _telemetryClient.TrackEvent("SubscriptionDropped", new Dictionary<string, string> {{ "Reason", reason.ToString() }});

            if (ex != null)
                _telemetryClient.TrackException(ex);

            Connect();
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore (read model)", new Dictionary<string, string> {
                { "Type", resolvedEvent.Event.EventType },
                { "EventNumber", resolvedEvent.Event.EventNumber.ToString() },
                { "Created (Epoch)", resolvedEvent.Event.CreatedEpoch.ToString() }
            });

            using (var scope = _scopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var @event = _mapper.Map<IEvent>(resolvedEvent);

                var hub = scope.ServiceProvider.GetService<IHubContext<ReduxEventHub, IReduxHub>>();
                await hub.Clients.All.SendAction("YOLO");

                await mediator.Publish(@event);
            }
        }

        public void Dispose()
        {
            EventStorePersistentSubscriptionBase.Stop(TimeSpan.FromSeconds(15));
        }
    }
}
