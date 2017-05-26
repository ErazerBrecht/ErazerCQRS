using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs;

namespace Erazer.Services.Queries.Repositories
{
    public interface IPriorityQueryRepository
    {
        Task<IList<IPriorityDto>> All();
    }
}