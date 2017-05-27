using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs;

namespace Erazer.Services.Queries.Repositories
{
    public interface ITicketQueryRepository 
    {
        Task<TicketDto> Find(string id);
        Task<TicketListDto> All();
        Task Update(TicketDto ticket);
    }
}
