using System.Text;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using MediatR;
using Newtonsoft.Json;
using Erazer.Shared;
using System;
using EasyNetQ;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventReciever : IEventReciever
    {
        private readonly IBus _bus;
        private readonly IMediator _mediator;

        public EventReciever(IBus bus, IMediator mediator)
        {
            _bus = bus;
            _mediator = mediator;
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
                // TODO LOG 
                throw;
            }
        }
    }
}
