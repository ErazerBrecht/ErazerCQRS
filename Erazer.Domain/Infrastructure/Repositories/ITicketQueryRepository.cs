using Erazer.Domain.Infrastructure.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Infrastructure.Repositories
{
    public interface ITicketQueryRepository 
    {
        Task<TicketDto> Find(string id);
        Task<List<TicketListDto>> All();
        Task Update(TicketDto ticket);
        Task Insert(TicketDto ticket);
    }
}
