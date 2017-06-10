using System.Text;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Erazer.Servicebus
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IQueueClient _queueClient;

        public EventPublisher(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }
        public async Task Publish<T>(T @event) where T : class, IEvent
        {
            var jsonEvent = JsonConvert.SerializeObject(@event, DefaultJsonSerializerSettings.DefaultSettings);
            var message = new Message(Encoding.UTF8.GetBytes(jsonEvent));
            
            // Send the message to the queue
            await _queueClient.SendAsync(message);
        }
    }
}
