using System.Threading.Tasks;
using Erazer.Services.Events.Mappings;

namespace Erazer.Services.Events
{
    public interface IWebsocketEmittor
    {
        Task Emit(ReduxAction action);
    }
}