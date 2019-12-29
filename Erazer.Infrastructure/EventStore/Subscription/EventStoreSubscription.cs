using Erazer.Infrastructure.Logging;
using Erazer.Infrastructure.MongoDb;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class Subscription : ISubscription
    {
        private readonly IEventStore _eventStore;
        private readonly IPositionRepository _positionRepository;
        private readonly ITelemetry _telemetryClient;
        private readonly IServiceProvider _provider;
        private readonly ILogger<Subscription> _logger;

        private IDisposable _subscription;

        public Subscription(IEventStore eventStore, IPositionRepository positionRepository,
            ITelemetry telemetryClient, IEventTypeMapping eventMap, IServiceProvider provider,
            ILogger<Subscription> logger)
        {
            _eventStore = eventStore;
            _positionRepository = positionRepository;
            _telemetryClient = telemetryClient;
            _provider = provider;
            _logger = logger;
        }

        public async Task Connect()
        {
            var position = await _positionRepository.GetCurrentPosition();
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

            using (var scope = _provider.CreateScope())
            {
                var session = scope.ServiceProvider.GetRequiredService<IMongoDbSession>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await session.StartTransaction();

                try
                {
                    await mediator.Publish(@event, token);
                    await _positionRepository.SetCurrentPosition(session, position, DateTimeOffset.UtcNow);

                    await session.Commit();
                    await session.ExecuteSideEffects();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Event handling failed for {@event.GetType()} on position {position.ToString()}");
                    await session.Abort();

                    throw;
                }
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
            _logger.LogInformation("Finished subscribing on EventStore");
        }
    }
}