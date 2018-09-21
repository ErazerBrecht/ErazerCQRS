using System;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public interface ISubscription: IDisposable
    {
        void Connect(long? position);
    }
}
