using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketPriorityEventHandler : IAsyncRequestHandler<TicketPriorityEvent>
    {
        private readonly ITicketQueryRepository _ticketRepository;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task Handle(TicketPriorityEvent message)
        {
            var ticket = await _ticketRepository.Find(message.AggregateRootId.ToString());
            ticket.PriorityId = message.ToPriorityId;
            await _ticketRepository.Update(ticket);
        }
    }
}
