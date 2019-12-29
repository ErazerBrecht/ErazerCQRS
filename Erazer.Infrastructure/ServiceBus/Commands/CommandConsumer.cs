using Erazer.Infrastructure.Logging;
using Erazer.Messages.Commands;

namespace Erazer.Infrastructure.ServiceBus.Commands
{
    public class CommandConsumer<T> : IConsumer<T> where T : class, ICommand
    {
        private readonly IServiceProvider _provider;
        private readonly ITelemetry _telemetry;

        public CommandConsumer(IServiceProvider provider, ITelemetry telemetry)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _telemetry = telemetry ?? throw new ArgumentNullException(nameof(telemetry));
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            var typeName = context.Message.GetType().Name;
            var messageId = context.MessageId;
            var now = DateTimeOffset.Now;
            var sw = Stopwatch.StartNew();

            try
            {
                using (var scope = _provider.CreateScope())
                {
                    _telemetry.TrackEvent($"Received command {typeName} | {messageId}");

                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(context.Message);
                    
                    _telemetry.TrackSpan("Command consumer", $"Handled command {typeName} | {messageId}", now, sw.Elapsed, true);
                }
            }
            catch (Exception e)
            {
                _telemetry.TrackSpan("Command consumer", $"Exception when executing command {context.Message} | {messageId}", now, sw.Elapsed, false);
                throw;
            }
        }
    }
}
