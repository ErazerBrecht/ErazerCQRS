using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Domain.Events;
using Erazer.Framework.FrontEnd;
using Erazer.Infrastructure.MongoDb;
using Erazer.Messages.IntegrationEvents.Events;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Web.ReadAPI.DomainEvents.EventHandlers.Redux;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.DomainEvents.EventHandlers
{
    public class TicketPriorityEventHandler : AsyncNotificationHandler<TicketPriorityDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMongoDbSession _session;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository, IPriorityQueryRepository priorityRepository, ITicketEventQueryRepository eventRepository, IMapper mapper, 
            IWebsocketEmittor websocketEmittor, IIntegrationEventPublisher eventPublisher, IMongoDbSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        protected override async Task HandleCore(TicketPriorityDomainEvent message)
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

            await UpdateDb(ticket, ticketEvent);
            _session.AddSideEffect(() => SideEffects(ticket, ticketEvent));
        }

        private Task UpdateDb(TicketDto ticketDto, PriorityEventDto eventDto)
        {
            return Task.WhenAll(_ticketRepository.Update(ticketDto), _eventRepository.Add(eventDto));
        }

        private Task SideEffects(TicketDto ticket, PriorityEventDto ticketEvent)
        {
            return Task.WhenAll(EmitToFrontEnd(ticketEvent), AddOnBus(ticket, ticketEvent));
        }

        private Task EmitToFrontEnd(PriorityEventDto eventDto)
        {
            return _websocketEmittor.Emit(
                new ReduxUpdatePriorityAction(_mapper.Map<TicketPriorityEventViewModel>(eventDto)));
        }

        private Task AddOnBus(TicketDto ticketDto, PriorityEventDto eventDto)
        {
            var integrationEvent = new TicketPriorityIntegrationEvent(
                ticketDto.Priority.Id,
                ticketDto.Priority.Name,
                ticketDto.Id,
                ticketDto.Title,
                eventDto.Id,
                eventDto.Created,
                eventDto.UserId
            );

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}
