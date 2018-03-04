using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Events
{
    public interface IEventPublisher
    {
        Task Publish(byte[] @event);
        Task Publish(IEnumerable<byte[]> events);
    }
}
