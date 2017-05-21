using System.Threading.Tasks;

namespace Erazer.DAL.EF.Repositories.Base
{
    public abstract class BaseRepository<T> where T : class 
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
