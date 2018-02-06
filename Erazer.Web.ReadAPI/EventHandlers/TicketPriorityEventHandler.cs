using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using AutoMapper;
using MediatR;
using Erazer.Web.ReadAPI.ViewModels.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Framework.FrontEnd;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Web.ReadAPI.EventHandlers.Redux;

namespace Erazer.Web.ReadAPI.EventHandlers
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
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
        }

        public async Task Handle(TicketPriorityEvent message)
        {
            var ticketTask = _ticketRepository.Find(message.AggregateRootId.ToString());
            var oldPriorityTask = _priorityRepository.Find(message.FromPriorityId);
            var newPriorityTask = _priorityRepository.Find(message.ToPriorityId);

            await Task.WhenAll(ticketTask, oldPriorityTask, newPriorityTask);

            var ticket = ticketTask.Result;
            var oldPriority = oldPriorityTask.Result;
            var newPriority = newPriorityTask.Result;

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
                _websocketEmittor.Emit(new ReduxUpdatePriorityAction(_mapper.Map<TicketPriorityEventViewModel>(ticketEvent))),
                _ticketRepository.Update(ticket),
                _eventRepository.Add(ticketEvent));

        }
    }
}
