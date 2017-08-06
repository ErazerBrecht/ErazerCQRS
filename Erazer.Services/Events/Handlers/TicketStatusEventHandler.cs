using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketStatusEventHandler : IAsyncNotificationHandler<TicketStatusEvent>
    {
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;

        public TicketStatusEventHandler(ITicketQueryRepository ticketRepository, IStatusQueryRepository statusRepository, ITicketEventQueryRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _statusRepository = statusRepository;
            _eventRepository = eventRepository;
        }

        public async Task Handle(TicketStatusEvent message)
        {
            var ticket = await _ticketRepository.Find(message.AggregateRootId.ToString());
            var oldStatus = await _statusRepository.Find(message.FromStatusId);
            var newStatus = await _statusRepository.Find(message.ToStatusId);

            // Update ticket in ReadModel
            ticket.Status = newStatus;

            // Add ticket event in ReadModel
            var ticketEvent = new TicketEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                Event = new StatusEventDto(oldStatus, newStatus)
            };

            await _ticketRepository.Update(ticket);
            await _eventRepository.Add(ticketEvent);

        }
    }
}
