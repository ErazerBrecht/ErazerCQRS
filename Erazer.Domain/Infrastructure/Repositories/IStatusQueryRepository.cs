using Erazer.Domain.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Infrastructure.Repositories
{
    public interface IStatusQueryRepository
    {
        Task<IList<StatusDto>> All();
        Task<StatusDto> Find(string id);
        Task<bool> Any();

        Task Add(StatusDto status);
    }
}