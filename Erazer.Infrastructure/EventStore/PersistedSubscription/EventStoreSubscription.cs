using AutoMapper;
using Erazer.Framework.Events;
using MediatR;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;
using SqlStreamStore.Subscriptions;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public class EventStoreSubscription : ISubscription
    {
        private readonly IStreamStore _eventStoreConnection;
        private readonly IServiceProvider _provider;
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger<EventStoreSubscription> _logger;

        private IAllStreamSubscription _subscription;

        public EventStoreSubscription(IStreamStore eventStoreConnection, TelemetryClient telemeteryClient, IServiceProvider provider, ILogger<EventStoreSubscription> logger)
        {
            _eventStoreConnection = eventStoreConnection;
            _telemetryClient = telemeteryClient;
            _provider = provider;
            _logger = logger;
        }


        public void Connect()
        {
           _subscription = _eventStoreConnection.SubscribeToAll(null, EventAppeared, SubscriptionDropped);
        }

        private void SubscriptionDropped(IAllStreamSubscription subscription, SubscriptionDroppedReason reason, Exception ex)
        {
            _telemetryClient.TrackEvent("SubscriptionDropped", new Dictionary<string, string> { { "Reason", reason.ToString() } });

            if (ex != null)
            {
                _telemetryClient.TrackException(ex);
                _logger.LogError(ex, "Subscription dropped!", reason.ToString());
            }

            Task.Delay(5000).Wait();
            Connect();
        }

        private async Task EventAppeared(IAllStreamSubscription subscription, StreamMessage resolvedEvent, CancellationToken token)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore subsription", new Dictionary<string, string> {
                { "Type", resolvedEvent.Type },
                { "EventNumber", resolvedEvent.StreamVersion.ToString() },
                { "Created (Epoch)", resolvedEvent.CreatedUtc.ToLongDateString() }
            });

            using (var scope = _provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var json = await resolvedEvent.GetJsonData(token);
                var @event = JsonConvert.DeserializeObject<IDomainEvent>(json, JsonSettings.DefaultSettings);
                await mediator.Publish(@event, token);
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
            _logger.LogInformation("Finished subscribing on EventStore");
        }
    }
}
