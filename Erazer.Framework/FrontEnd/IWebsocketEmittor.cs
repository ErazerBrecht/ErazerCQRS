
using System.Threading.Tasks;

namespace Erazer.Framework.FrontEnd
{
    public interface IWebsocketEmittor
    {
        Task TestEmit();
        Task Emit<T>(ReduxAction<T> action) where T : IViewModel;
    }
}