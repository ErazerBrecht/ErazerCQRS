using System.Collections.Generic;
using System.Threading.Tasks;
using Erazer.DAL.ReadModel;

namespace Erazer.DAL.Dapper.Repositories.Base
{
    public interface ITicketEventRepository
    {
        Task<IList<TicketEventDto>> Find(string ticketId);

    }
}
