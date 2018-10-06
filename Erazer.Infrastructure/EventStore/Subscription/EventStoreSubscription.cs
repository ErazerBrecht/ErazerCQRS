using Erazer.Framework.Events;
using MediatR;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Infrastructure.MongoDb;
using Erazer.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;
using SqlStreamStore.Subscriptions;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class Subscription : ISubscription
    {
        private readonly IStreamStore _eventStoreConnection;
        private readonly IPositionRepository _positionRepository;
        private readonly TelemetryClient _telemetryClient;
        private readonly IEventTypeMapping _eventMap;
        private readonly IServiceProvider _provider;
        private readonly ILogger<Subscription> _logger;

        private IAllStreamSubscription _subscription;

        public Subscription(IStreamStore eventStoreConnection, IPositionRepository positionRepository, TelemetryClient telemeteryClient, IEventTypeMapping eventMap, 
            IServiceProvider provider, ILogger<Subscription> logger)
        {
            _eventStoreConnection = eventStoreConnection;
            _positionRepository = positionRepository;
            _telemetryClient = telemeteryClient;
            _eventMap = eventMap;
            _provider = provider;
            _logger = logger;
        }

        public void Connect(long? position)
        {
            _logger.LogDebug($"Started subscribing from position {position}");
           _subscription = _eventStoreConnection.SubscribeToAll(position, EventAppeared, SubscriptionDropped);
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
            Connect(subscription.LastPosition);
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
                var session = scope.ServiceProvider.GetRequiredService<IMongoDbSession>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await session.StartTransaction();

                var json = await resolvedEvent.GetJsonData(token);
                var eventType = _eventMap.GetType(resolvedEvent.Type);
                var @event = (IDomainEvent) JsonConvert.DeserializeObject(json, eventType, JsonSettings.DefaultSettings);
                @event.Version = resolvedEvent.StreamVersion;

                try
                {
                    await mediator.Publish(@event, token);
                    await _positionRepository.SetCurrentPosition(session, resolvedEvent.Position, DateTimeOffset.UtcNow);

                    await session.Commit();
                    await session.ExecuteSideEffects();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Event handling failed for {@event.GetType()}");
                    await session.Abort();

                    throw;
                }      
            }
        }

        public void Dispose()
        {
            _subscription.Dispose();
            _logger.LogInformation("Finished subscribing on EventStore");
        }
    }
}
