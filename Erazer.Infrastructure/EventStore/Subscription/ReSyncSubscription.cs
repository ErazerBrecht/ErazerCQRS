using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.ReadStore;
using Erazer.Read.Application.Infrastructure;
using Erazer.Syncing.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class ReSyncSubscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IDbQuery<PositionDto> _positionDbQuery;
        private readonly ITelemetry _telemetryClient;
        private readonly ILogger<ReSyncSubscription> _logger;
        private readonly IServiceProvider _provider;

        public ReSyncSubscription(IEventStore eventStore, IDbQuery<PositionDto> positionDbQuery, IServiceCollection serviceCollection,
            ITelemetry telemetryClient, ILogger<ReSyncSubscription> logger)
        {
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            _positionDbQuery = positionDbQuery ?? throw new ArgumentNullException(nameof(positionDbQuery));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            serviceCollection.AddScoped<IDbUnitOfWork, DbBatchUnitOfWork>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        public async Task Connect()
        {
            var isEnd = false;
            var position = await _positionDbQuery.Single(_ => true);
            var nextPosition = (position?.CheckPoint ?? -1) + 1;

            _telemetryClient.TrackEvent("Started 'ReSync' subscription!", new Dictionary<string, string>
            {
                {"Position", nextPosition.ToString()}
            });

            do
            {
                using var scope = _provider.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                
                try
                {
                    var result = (await _eventStore.GetAll(nextPosition, 2500));
                    var eventEnvelopes = result.EventEnvelopes.ToList();

                    if (!eventEnvelopes.Any() && result.IsEnd)
                    {
                        isEnd = true;
                        continue;
                    }

                    foreach (var eventEnvelope in eventEnvelopes)
                        await EventAppeared(eventEnvelope, mediator);
                    
                    var lastPosition = eventEnvelopes.Last().Position;
                    await ctx.Commit(lastPosition);

                    nextPosition = lastPosition + 1;
                    isEnd = result.IsEnd;
                }
                catch (Exception ex)
                {
                    _telemetryClient.TrackEvent("SubscriptionDropped");
                    _logger.LogError(ex, "Subscription dropped!");

                    await Task.Delay(1500);
                }
            } while (!isEnd);

            _telemetryClient.TrackEvent("ReSync subscription has caught up to the end of the stream!");
        }

        private async Task EventAppeared(IEventEnvelope<IEvent> eventEnvelope, IMediator mediator, CancellationToken token = default)
        {
            _telemetryClient.TrackEvent("New event appeared from EventStore subscription",
                new Dictionary<string, string>
                {
                    {"Type", eventEnvelope.Event.GetType().Name},
                    {"Position", eventEnvelope.Position.ToString()},
                    {"StreamVersion", eventEnvelope.Version.ToString()},
                    {"Created (Epoch)", eventEnvelope.Created.ToString()}
                });

            try
            {
                await mediator.Publish(eventEnvelope, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Event handling failed for {eventEnvelope.GetType()} on position {eventEnvelope.Position.ToString()}");
                throw;
            }
        }

        public void Dispose()
        {
        }
    }
}