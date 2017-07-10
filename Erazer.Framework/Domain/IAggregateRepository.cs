using System;
using System.Threading.Tasks;

namespace Erazer.Framework.Domain
{
    public interface IAggregateRepository
    {
        Task<T> Get<T>(Guid aggregateId) where T : AggregateRoot;
        Task Save<T>(T aggregate) where T : AggregateRoot;
    }
}
