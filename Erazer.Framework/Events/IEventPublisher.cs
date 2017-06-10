using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : class, IEvent;
    }
}
