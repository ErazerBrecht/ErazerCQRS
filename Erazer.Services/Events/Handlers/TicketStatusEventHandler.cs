using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;
using AutoMapper;
using Erazer.Services.Events.Redux;
using Erazer.Services.Queries.ViewModels.Events;

namespace Erazer.Services.Events.Handlers
{
    public class TicketStatusEventHandler : IAsyncNotificationHandler<TicketStatusEvent>
    {
        private readonly IMapper _mapper;
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;

        public TicketStatusEventHandler(IMapper mapper, IWebsocketEmittor websocketEmittor, ITicketQueryRepository ticketRepository, IStatusQueryRepository statusRepository, ITicketEventQueryRepository eventRepository)
        {
            _mapper = mapper;
            _websocketEmittor = websocketEmittor;
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
            var ticketEvent = new StatusEventDto(oldStatus, newStatus)
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
            };

            await Task.WhenAll(
                    _websocketEmittor.Emit(new ReduxAction(ReduxActionTypeConstants.UpdateTicketPriority, _mapper.Map<TicketStatusEventViewModel>(ticketEvent))),
                    _ticketRepository.Update(ticket),
                    _eventRepository.Add(ticketEvent));
        }
    }
}
