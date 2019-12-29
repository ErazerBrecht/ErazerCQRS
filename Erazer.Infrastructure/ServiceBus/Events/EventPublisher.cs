using Erazer.Infrastructure.Logging;
using Erazer.Messages.IntegrationEvents;
using Erazer.Messages.IntegrationEvents.Infrastructure;

namespace Erazer.Infrastructure.ServiceBus.Events
{
    public class EventPublisher: IIntegrationEventPublisher
    {
        private readonly IBusControl _bus;
        private readonly ITelemetry _telemetry;

        public EventPublisher(IBusControl bus, ITelemetry telemetry)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IIntegrationEvent
        {
            var now = DateTimeOffset.Now;
            var sw = Stopwatch.StartNew();
            var type = @event.GetType().Name;

            _telemetry.TrackTrace($"Publishing event {type} to the bus");

            try
            {
                await _bus.Publish(@event, cancellationToken);
                _telemetry.TrackDependency("ServiceBus", "RabbitMQ - Event", $"Publishing event {type} succeeded", now, sw.Elapsed, true);
            }
            catch (Exception)
            {
                _telemetry.TrackDependency("ServiceBus", "RabbitMQ - Event", $"Publishing event {type} failed", now, sw.Elapsed, false);
                throw;
            }
        }

        public Task Publish<T>(IEnumerable<T> events, CancellationToken cancellationToken) where T : class, IIntegrationEvent
        {
            var tasks = events.Select(e => Publish(e, cancellationToken));
            return Task.WhenAll(tasks);
        }
    }
}
