using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel;

namespace Erazer.DAL.Dapper.Repositories.Base
{
    public interface IPriorityRepository
    {
        Task<IList<PriorityDto>> All();
    }
}