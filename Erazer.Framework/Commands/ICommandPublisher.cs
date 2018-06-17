using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Framework.Commands
{
    public interface ICommandPublisher
    {
        Task Publish<T>(T command, string endpoint) where T : class, ICommand;
        Task Publish<T>(IEnumerable<T> commands, string endpoint) where T : class, ICommand;
    }
}
