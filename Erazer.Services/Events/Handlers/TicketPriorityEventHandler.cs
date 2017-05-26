using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketPriorityEventHandler : IAsyncRequestHandler<TicketPriorityEvent>
    {
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository, IPriorityQueryRepository priorityRepository)
        {
            _ticketRepository = ticketRepository;
            _priorityRepository = priorityRepository;
        }

        public async Task Handle(TicketPriorityEvent message)
        {
            var ticket = await _ticketRepository.Find(message.AggregateRootId.ToString());
            var priority = await _priorityRepository.Find(message.ToPriorityId.ToString());
            ticket.Priority = priority;
            await _ticketRepository.Update(ticket);
        }
    }
}
