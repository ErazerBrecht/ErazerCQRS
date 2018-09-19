using System;
using Erazer.Framework.Domain;

namespace Erazer.Infrastructure.EventStore.PersistedSubscription
{
    public interface ISubscription: IDisposable
    {
        void Connect();
    }
}
