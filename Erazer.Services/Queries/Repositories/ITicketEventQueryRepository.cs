using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.Services.Queries.DTOs.Events;

namespace Erazer.Services.Queries.Repositories
{
    public interface ITicketEventQueryRepository
    {
        Task<IList<TicketEventDto>> Find(string ticketId);
        Task Add(TicketEventDto ticketEvent);
    }
}
