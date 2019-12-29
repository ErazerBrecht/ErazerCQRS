using Erazer.Infrastructure.Logging;
using Erazer.Messages.Commands;
using Erazer.Messages.Commands.Infrastructure;

namespace Erazer.Infrastructure.ServiceBus.Commands
{
    public class CommandPublisher: ICommandPublisher
    {
        private readonly IBusControl _bus;
        private readonly ITelemetry _telemetry;

        public CommandPublisher(IBusControl bus, ITelemetry telemetry)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }

        public async Task Publish<T>(T message, string endpoint, CancellationToken cancellationToken) where T : class, ICommand
        {
            var now = DateTimeOffset.Now;
            var sw = Stopwatch.StartNew();
            var type = message.GetType().Name;
            var sendToUri = GetCommandRabbitMqAddress(_bus.Address, endpoint);

            _telemetry.TrackTrace($"Sending command {type} to {sendToUri}");
            
            try
            {
                var endPoint = await _bus.GetSendEndpoint(sendToUri);
                await endPoint.Send(message, cancellationToken);
                _telemetry.TrackDependency("ServiceBus", "RabbitMQ - Command", $"Sent command {type} to {sendToUri} succeeded", now, sw.Elapsed, true);
            }
            catch (Exception)
            {
                _telemetry.TrackDependency("ServiceBus", "RabbitMQ - Command", $"Sent command {type} to {sendToUri} failed", now, sw.Elapsed, false);
                throw;
            }
        }

        public Task Publish<T>(IEnumerable<T> commands, string endpoint, CancellationToken cancellationToken) where T : class, ICommand
        {
            var tasks = commands.Select(e => Publish(e, endpoint, cancellationToken));
            return Task.WhenAll(tasks);
        }
        
        private Uri GetCommandRabbitMqAddress(Uri busUri, string endpoint)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));

            return busUri.Segments.Length == 3
                ? new Uri($"{busUri.Scheme}://{busUri.Host}/{busUri.Segments[1]}{endpoint}")
                : new Uri($"{busUri.Scheme}://{busUri.Host}/{endpoint}");
        }
    }
}
