using System;
using System.Threading.Tasks;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface ISubscription: IDisposable
    {
        Task Connect();
    }
}
