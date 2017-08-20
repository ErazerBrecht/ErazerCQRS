using System.Threading.Tasks;
using Erazer.Services.Events.Redux;

namespace Erazer.Services.Events
{
    public interface IWebsocketEmittor
    {
        Task Emit(ReduxAction action);
    }
}