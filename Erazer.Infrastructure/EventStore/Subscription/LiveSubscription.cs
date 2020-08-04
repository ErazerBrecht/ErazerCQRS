using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ReadStore;
using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class LiveSubscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IDbQuery<SubscriptionDto> _subscriptionDbQuery;
        private readonly IServiceProvider _provider;
        private readonly ITelemetry _telemetryClient;
        private readonly ILogger<LiveSubscription> _logger;

        private IDisposable _subscription;

        public LiveSubscription(IEventStore eventStore, IDbQuery<SubscriptionDto> subscriptionDbQuery,
            IServiceProvider provider, ITelemetry telemetryClient, ILogger<LiveSubscription> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _subscriptionDbQuery = subscriptionDbQuery ?? throw new ArgumentNullException(nameof(subscriptionDbQuery));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Connect()
        {
            var position = await _subscriptionDbQuery.Single(_ => true);
            _logger.LogInformation($"Started a 'Live' subscription from position {position?.CheckPoint.ToString() ?? "NULL"}");
            _subscription = _eventStore.Subscribe(position?.CheckPoint, EventAppeared, SubscriptionDropped, HasCaughtUp);
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

        private async Task EventAppeared(IEventEnvelope<IEvent> eventEnvelope, CancellationToken token)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore subscription", new Dictionary<string, string>
            {
                {"Type", eventEnvelope.Event.GetType().Name},
                {"Position", eventEnvelope.Position.ToString()},
                {"StreamVersion", eventEnvelope.Version.ToString()},
                {"Created (Epoch)", eventEnvelope.Created.ToString()}
            });

            using var scope = _provider.CreateScope();
            var ctx = (SubscriptionContext) scope.ServiceProvider.GetRequiredService<ISubscriptionContext>();
            ctx.InitializeContext(SubscriptionType.Live);
            
            var session = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                await session.Start();
                await mediator.Publish(eventEnvelope, token);
                
                var updatedSubscription = new SubscriptionDto
                {
                    Id = "ERAZER_CQRS_SUBSCRIPTION_POSITION",
                    CheckPoint = eventEnvelope.Position,
                    UpdatedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Type = SubscriptionType.Live
                };
            
                await session.Subscriptions.Mutate(updatedSubscription, token);
                await session.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Event handling failed for {eventEnvelope.GetType()} on position {eventEnvelope.Position.ToString()}");
                throw;
            }
        }
        
        private void HasCaughtUp(bool hasCaughtUp)
        {
            if (hasCaughtUp)
                _telemetryClient.TrackEvent("Live subscription has caught up to the end of the stream!");

        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _logger.LogInformation("Finished subscribing on EventStore");
        }
    }
}