using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Erazer.Infrastructure.Logging;
using Erazer.Messages.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Infrastructure.ServiceBus.Events
{
    public class EventConsumer<T> : IConsumer<T> where T : class, IIntegrationEvent
    {
        private readonly IServiceProvider _provider;
        private readonly ITelemetry _telemetry;

        public EventConsumer(IServiceProvider provider, ITelemetry telemetry)
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
                using var scope = _provider.CreateScope();
                _telemetry.TrackEvent($"Received event {typeName} | {messageId}");

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(context.Message);
                    
                _telemetry.TrackSpan("Event consumer", $"Handled event {typeName} | {messageId}", now, sw.Elapsed, true);
            }
            catch (Exception)
            {
                _telemetry.TrackSpan("Event consumer", $"Exception when executing event {context.Message} | {messageId}", now, sw.Elapsed, false);
                throw;
            }
        }
    }
}
