using System.Threading.Tasks;
using MediatR;
using System;
using MassTransit;
using Microsoft.Extensions.Logging;
using Erazer.Messages.IntegrationEvents;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventConsumer<T> : IConsumer<T> where T : class, IIntegrationEvent
    {
        private readonly ILogger<EventConsumer<T>> _logger;
        private readonly IMediator _mediator;

        public EventConsumer(ILogger<EventConsumer<T>> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            var @event = context.Message;
            try
            {
                _logger.LogInformation($"Received '{@event.GetType().Name}' command from {context.SourceAddress}");
                await _mediator.Publish(@event);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing event {@event}");
                throw;
            }
        }
    }
}
