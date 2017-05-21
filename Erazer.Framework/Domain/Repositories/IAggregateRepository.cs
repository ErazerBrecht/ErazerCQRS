using System.Threading.Tasks;

namespace Erazer.Framework.Domain.Repositories
{
    public interface IAggregateRepository<T> where T : AggregateRoot
    {
        Task<T> Get(string aggregateId);
        Task Save(T aggregate);
    }
}
