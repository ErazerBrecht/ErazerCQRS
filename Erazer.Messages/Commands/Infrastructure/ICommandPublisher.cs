using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Erazer.Messages.Commands.Infrastructure
{
    public interface ICommandPublisher
    {
        Task Publish<T>(T command, string endpoint, CancellationToken cancellationToken = default) where T : class, ICommand;
        Task Publish<T>(IEnumerable<T> commands, string endpoint, CancellationToken cancellationToken = default) where T : class, ICommand;
    }
}
