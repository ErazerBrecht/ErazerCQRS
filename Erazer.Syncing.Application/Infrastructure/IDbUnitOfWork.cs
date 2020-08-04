using System.Threading.Tasks;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Syncing.Models;

namespace Erazer.Syncing.Infrastructure
{
    public interface IDbUnitOfWork
    {
        public IDbRepository<SubscriptionDto> Subscriptions { get; }
        
        public IDbRepository<StatusDto> Statuses { get; }
        public IDbRepository<PriorityDto> Priorities { get; }
        
        public IDbRepository<TicketListDto> TicketList { get; }
        public IDbRepository<TicketDto> Tickets { get; }

        public Task Start();
        public Task Commit();
    }
}