using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketPriorityEventHandler : IAsyncNotificationHandler<TicketPriorityEvent>
    {
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly ITicketEventQueryRepository _eventRepository;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository, IPriorityQueryRepository priorityRepository, ITicketEventQueryRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _priorityRepository = priorityRepository;
            _eventRepository = eventRepository;
        }

        public async Task Handle(TicketPriorityEvent message)
        {
            var ticket = await _ticketRepository.Find(message.AggregateRootId.ToString());
            var oldPriority = await _priorityRepository.Find(message.FromPriorityId);
            var newPriority = await _priorityRepository.Find(message.ToPriorityId);

            // Update ticket in ReadModel
            ticket.Priority = newPriority;

            // Add ticket event in ReadModel
            var ticketEvent = new PriorityEventDto(oldPriority, newPriority)
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                
            };

            await _ticketRepository.Update(ticket);
            await _eventRepository.Add(ticketEvent);

        }
    }
}
