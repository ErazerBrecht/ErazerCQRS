using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class ReSyncSubscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IDbQuery<PositionDto> _positionDbQuery;
        private readonly ITelemetry _telemetryClient;
        private readonly IMediator _mediator;
        private readonly IDbUnitOfWork _ctx;
        private readonly ILogger<ReSyncSubscription> _logger;

        private long _lastPosition;
        private IDisposable _subscription;

        public ReSyncSubscription(IEventStore eventStore, IDbQuery<PositionDto> positionDbQuery, ITelemetry telemetryClient, 
            IMediator mediator, IDbUnitOfWork ctx, ILogger<ReSyncSubscription> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _positionDbQuery = positionDbQuery ?? throw new ArgumentNullException(nameof(positionDbQuery));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Connect()
        {
            var position = await _positionDbQuery.Single(_ => true);
            var checkpoint = position?.CheckPoint ?? -1;
            _logger.LogInformation($"Started a 'resync' subscription from position {checkpoint}");
            _subscription = _eventStore.Subscribe(position?.CheckPoint, EventAppeared, SubscriptionDropped);

            _lastPosition = checkpoint;
        }

        private void SubscriptionDropped(Exception ex)
        {
            _telemetryClient.TrackEvent("SubscriptionDropped");

            if (ex != null)
            {
                _logger.LogError(ex, "Subscription dropped!");
            }

            Task.Delay(1500).Wait();
            Connect().Wait();
        }

        private async Task EventAppeared(long position, IDomainEvent @event, CancellationToken token)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore subscription", new Dictionary<string, string>
            {
                {"Type", @event.GetType().Name},
                {"Position", position.ToString()},
                {"StreamVersion", @event.Version.ToString()},
                {"Created (Epoch)", @event.Created.ToLongDateString()}
            });

           try
            {
                await _mediator.Publish(@event, token);

                if (position >= _lastPosition + 2500)
                {
                    await _ctx.Commit(position);
                    _lastPosition = position;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Event handling failed for {@event.GetType()} on position {position.ToString()}");
                throw;
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _logger.LogInformation("Finished subscribing on EventStore");
        }
    }
}