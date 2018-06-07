using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Commands
{
    public interface ICommandPublisher<T> where T: class, ICommand
    {
        Task Publish(T command);
        Task Publish(IEnumerable<T> commands);
    }
}
