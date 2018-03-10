using System.Text;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using MediatR;
using Newtonsoft.Json;
using Erazer.Shared;
using System;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventReciever : IEventReciever
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;
        private readonly ILogger<EventReciever> _logger;

        public EventReciever(IBus bus, IMediator mediator, ILogger<EventReciever> logger)
        {
            _bus = bus;
            _mediator = mediator;
            _logger = logger;
        }

        public void RegisterEventReciever()
        {
            _bus.SubscribeAsync<byte[]>("test", ProcessEvents);
        }

        private async Task ProcessEvents(byte[] message)
        {
            // Deserialze event
            var messageBody = Encoding.UTF8.GetString(message);
            var @event = JsonConvert.DeserializeObject<INotification>(messageBody, JsonSettings.DefaultSettings);

            // Process event
            try
            {
                await _mediator.Publish(@event);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Exception when executing event {@event.ToString()}");
                throw;
            }
        }
    }
}
