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
    public class TicketPriorityEventHandler : IAsyncNotificationHandler<TicketPriorityEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmittor _websocketEmittor;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository, IPriorityQueryRepository priorityRepository, ITicketEventQueryRepository eventRepository, IMapper mapper, IWebsocketEmittor websocketEmittor)
        {
            _ticketRepository = ticketRepository;
            _priorityRepository = priorityRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _websocketEmittor = websocketEmittor;
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


            await Task.WhenAll(
                _websocketEmittor.Emit(new ReduxAction(ReduxActionTypeConstants.UpdateTicketPriority, _mapper.Map<TicketPriorityEventViewModel>(ticketEvent))),
                _ticketRepository.Update(ticket),
                _eventRepository.Add(ticketEvent));

        }
    }
}
