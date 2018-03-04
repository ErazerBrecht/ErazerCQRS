using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Framework.Events;
using EasyNetQ;
using System;

namespace Erazer.Infrastructure.ServiceBus
{
    public class EventPublisher : IEventPublisher, IDisposable
    {
        private readonly IBus _bus;

        public EventPublisher(IBus bus)
        {
            _bus = bus;
        }

        public Task Publish(byte[] @event)
        {
            return _bus.PublishAsync(@event);
        }

        public Task Publish(IEnumerable<byte[]> events)
        {
            var tasks = events.Select(e => _bus.PublishAsync(e));
            return Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}
