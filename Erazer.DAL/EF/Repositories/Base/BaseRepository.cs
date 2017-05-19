using System.Threading.Tasks;
using Erazer.Services.Events.Entities;
using Erazer.Services.Events.Repositories;

namespace Erazer.DAL.EF.Repositories.Base
{
    public abstract class BaseRepository<T> : IBaseEventRepository<T> where T : class, IEventEntity 
    {
        protected ErazerEventContext Context;

        protected BaseRepository(ErazerEventContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }


        public async Task Commit()
        {
           await Context.SaveChangesAsync();
        }
    }
}
