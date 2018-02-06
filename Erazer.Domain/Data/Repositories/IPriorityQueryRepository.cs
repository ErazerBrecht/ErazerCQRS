using Erazer.Domain.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Data.Repositories
{
    public interface IPriorityQueryRepository
    {
        Task<IList<PriorityDto>> All();
        Task<PriorityDto> Find(string id);
        Task<bool> Any();

        Task Add(PriorityDto status);
    }
}                                                                                                                    