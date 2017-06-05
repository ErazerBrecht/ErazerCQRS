using System;
using System.Threading.Tasks;

namespace Erazer.Framework.Domain
{
    public interface IAggregateRepository<T> where T : AggregateRoot
    {
        Task<T> Get(Guid aggregateId);
        Task Save(T aggregate);
    }
}
