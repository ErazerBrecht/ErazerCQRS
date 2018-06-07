using System;
using System.Threading.Tasks;
using Erazer.Framework.Domain;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public interface ISubscription<T>: IDisposable where T : AggregateRoot
    {
        Task Connect();
    }
}
