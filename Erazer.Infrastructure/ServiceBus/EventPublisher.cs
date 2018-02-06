using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Microsoft.Azure.ServiceBus;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IQueueClient _queueClient;

        public EventPublisher(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public Task Publish(byte[] @event)
        {
            var message = new Message(@event);          
            return _queueClient.SendAsync(message);
        }

        public Task Publish(IEnumerable<byte[]> events)
        {
            var messages = events.Select(jsonEvent => new Message(jsonEvent)).ToList();
            return _queueClient.SendAsync(messages);
        }

        public Task Close()
        {
            return _queueClient.CloseAsync();
        }
    }
}
