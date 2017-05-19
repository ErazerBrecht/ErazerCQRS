using System.Threading.Tasks;
using Erazer.Services.Events.Entities;

namespace Erazer.Services.Events.Repositories
{
    public interface IBaseEventRepository<T> where T : class, IEventEntity
    {
        void Add(T entity);
        Task Commit();
    }
}
