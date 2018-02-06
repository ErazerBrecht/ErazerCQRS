using Erazer.Domain.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Data.Repositories
{
    public interface IStatusQueryRepository
    {
        Task<IList<StatusDto>> All();
        Task<StatusDto> Find(string id);
        Task<bool> Any();

        Task Add(StatusDto status);
    }
}