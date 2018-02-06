using Erazer.Framework.Domain;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    // TODO: Is this interface in the correct place?
    public interface ISubscription<T> where T : AggregateRoot
    {
        void Connect();
    }
}
