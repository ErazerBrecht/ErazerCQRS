using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.MongoDb;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class Subscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IPositionStore _positionStore;
        private readonly ITelemetry _telemetryClient;
        private readonly IServiceProvider _provider;
        private readonly ILogger<Subscription> _logger;

        private IDisposable _subscription;

        public Subscription(IEventStore eventStore, IPositionStore positionStore,
            ITelemetry telemetryClient, IServiceProvider provider, ILogger<Subscription> logger)
        {
            _eventStore = eventStore;
            _positionStore = positionStore;
            _telemetryClient = telemetryClient;
            _provider = provider;
            _logger = logger;
        }

        public async Task Connect()
        {
            var position = await _positionStore.GetCurrentPosition();
            _logger.LogInformation($"Started subscribing from position {position?.CheckPoint ?? -1}");
            _subscription = _eventStore.Subscribe(position?.CheckPoint, EventAppeared, SubscriptionDropped);
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

            using var scope = _provider.CreateScope();
            var session = scope.ServiceProvider.GetRequiredService<IDbSession>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                await session.StartTransaction();

                await mediator.Publish(@event, token);
                await _positionStore.SetCurrentPosition(session, position, DateTimeOffset.UtcNow);

                await session.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Event handling failed for {@event.GetType()} on position {position.ToString()}");
                await session.Abort();

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