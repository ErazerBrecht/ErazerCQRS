using System.Threading.Tasks;
using MediatR;
using System;
using Erazer.Framework.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventReciever<T> : IConsumer<T> where T : class, IEvent
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventReciever<T>> _logger;

        public EventReciever(IMediator mediator, ILogger<EventReciever<T>> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            // Process event
            try
            {
                await _mediator.Publish(context.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing event {context.Message}");
                throw;
            }
        }
    }
}
