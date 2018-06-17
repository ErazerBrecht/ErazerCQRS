using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Messages.IntegrationEvents;
using Erazer.Messages.IntegrationEvents.Infrastructure;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventPublisher: IIntegrationEventPublisher
    {
        private readonly IEventBus _bus;

        public EventPublisher(IEventBus bus)
        {
            _bus = bus;
        }

        public Task Publish<T>(T @event) where T : class, IIntegrationEvent
        {
            return _bus.Publish(@event);
        }

        public Task Publish<T>(IEnumerable<T> events) where T : class, IIntegrationEvent
        {
            var tasks = events.Select(e => _bus.Publish(e));
            return Task.WhenAll(tasks);
        }
    }
}
