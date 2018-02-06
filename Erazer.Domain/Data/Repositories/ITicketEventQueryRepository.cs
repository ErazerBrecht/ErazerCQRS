using Erazer.Domain.Data.DTOs.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Erazer.Domain.Data.Repositories
{
    public interface ITicketEventQueryRepository
    {
        Task<IList<TicketEventDto>> Find(string ticketId);
        Task Add(TicketEventDto ticketEvent);
    }
}
