using System.Threading.Tasks;
using Erazer.DAL.ReadModel;

namespace Erazer.DAL.Dapper.Repositories.Base
{
    public interface ITicketRepository
    {
        Task<TicketDto> Find(string id);
        Task<TicketListDto> All();
    }
}
