using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventRepository
    {
        void AddEvent(IEvent @event);
        Task Commit();
    }
}
