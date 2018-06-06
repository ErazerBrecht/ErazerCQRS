using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventPublisher<T> where T : class, IEvent
    {
        Task Publish(T @event);
        Task Publish(IEnumerable<T> events);
    }
}
