using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs;

namespace Erazer.Services.Queries.Repositories
{
    public interface ITicketQueryRepository 
    {
        Task<TicketDto> Find(string id);
        Task<List<TicketListDto>> All();
        Task Update(TicketDto ticket);
    }
}
