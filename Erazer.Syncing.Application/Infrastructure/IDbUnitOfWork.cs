using System.Threading.Tasks;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;

namespace Erazer.Syncing.Infrastructure
{
    public interface IDbUnitOfWork
    {
        public IDbRepository<StatusDto> Statuses { get; }
        public IDbRepository<PriorityDto> Priorities { get; }
        
        public IDbRepository<TicketListDto> TicketList { get; }
        public IDbRepository<TicketDto> Tickets { get; }
        public IDbRepository<TicketEventDto> TicketEvents { get; }

        public Task Start();
        public Task Commit(long position);
    }
}