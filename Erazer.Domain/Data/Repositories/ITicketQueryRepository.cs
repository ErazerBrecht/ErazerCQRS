using Erazer.Domain.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Data.Repositories
{
    public interface ITicketQueryRepository 
    {
        Task<TicketDto> Find(string id);
        Task<List<TicketListDto>> All();
        Task Update(TicketDto ticket);
        Task Insert(TicketDto ticket);
    }
}
