using System;
using Erazer.Framework.Domain;

namespace Erazer.Framework.Cache
{
    public interface ICache
    {
        bool IsTracked(Guid id);
        void Set(Guid id, AggregateRoot aggregate);
        AggregateRoot Get(Guid id);
        void Remove(Guid id);
    }
}