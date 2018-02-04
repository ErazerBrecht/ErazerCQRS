﻿using AutoMapper;
using Erazer.Framework.Domain;
using Erazer.Framework.Events;
using EventStore.ClientAPI;
using MediatR;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.DAL.Events
{
    public class EventStoreSubscription<T> : ISubscription<T>, IDisposable where T : AggregateRoot
    {
        private const string _groupName = "erazercqrs";

        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly TelemetryClient _telemetryClient;

        private EventStorePersistentSubscriptionBase EventStorePersistentSubscriptionBase { get; set; }

        public EventStoreSubscription(IEventStoreConnection eventStoreConnection, IMapper mapper, IMediator mediator, TelemetryClient telemeteryClient)
        {
            _eventStoreConnection = eventStoreConnection;
            _mapper = mapper;
            _mediator = mediator;
            _telemetryClient = telemeteryClient;
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

        private Task EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent)
        {
            _telemetryClient.TrackEvent("New Event", new Dictionary<string, string> { { "Type", resolvedEvent.Event.EventType } });

            var @event = _mapper.Map<IEvent>(resolvedEvent);
            return _mediator.Publish(@event);
        }

        public void Dispose()
        {
            EventStorePersistentSubscriptionBase.Stop(TimeSpan.FromSeconds(15));
        }
    }
}
