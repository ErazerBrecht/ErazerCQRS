using System.Threading.Tasks;

namespace Erazer.Domain.Aggregates.Repositories
{
    public interface IAggregateRepository<T> where T : AggregateRoot
    {
        Task<T> Get(string aggregateId);
        Task Save(T aggregate);
    }
}
