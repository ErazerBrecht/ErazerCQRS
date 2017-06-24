using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : class, IEvent;
        Task Publish<T>(IEnumerable<T> events) where T : class, IEvent;
        Task Close();
    }
}
