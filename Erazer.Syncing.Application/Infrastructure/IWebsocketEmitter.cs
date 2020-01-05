using System.Threading.Tasks;
using Erazer.Syncing.SeedWork;

namespace Erazer.Syncing.Infrastructure
{
    public interface IWebsocketEmitter
    {
        Task Emit<T>(ReduxAction<T> action) where T : class;
    }
}