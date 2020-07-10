using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Infrastructure.Logging;
using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class LiveSubscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IDbQuery<PositionDto> _positionDbQuery;
        private readonly ITelemetry _telemetryClient;
        private readonly IServiceProvider _provider;
        private readonly ILogger<LiveSubscription> _logger;

        private IDisposable _subscription;

        public LiveSubscription(IEventStore eventStore, IDbQuery<PositionDto> positionDbQuery,
            ITelemetry telemetryClient, IServiceProvider provider, ILogger<LiveSubscription> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _positionDbQuery = positionDbQuery ?? throw new ArgumentNullException(nameof(positionDbQuery));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Connect()
        {
            var position = await _positionDbQuery.Single(_ => true);
            _logger.LogInformation($"Started a 'live' subscription from position {position?.CheckPoint ?? -1}");
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
            var session = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                await session.Start();
                await mediator.Publish(@event, token);
                await session.Commit(position);
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