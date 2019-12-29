using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Messages.IntegrationEvents.Models;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork.Redux;
using MediatR;

namespace Erazer.Syncing.Handlers
{
    public class TicketPriorityEventHandler : INotificationHandler<TicketPriorityDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketPriorityEventHandler(ITicketQueryRepository ticketRepository,
            IPriorityQueryRepository priorityRepository, ITicketEventQueryRepository eventRepository, IMapper mapper,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketPriorityDomainEvent message, CancellationToken cancellationToken)
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
                UpdateDb(ticket, ticketEvent),
                EmitToFrontEnd(ticketEvent),
                AddOnBus(ticket, ticketEvent)
            );
        }

        private async Task UpdateDb(TicketDto ticketDto, PriorityEventDto eventDto)
        {
            await _ticketRepository.Update(ticketDto);
            await _eventRepository.Add(eventDto);
        }

        private Task EmitToFrontEnd(PriorityEventDto eventDto)
        {
            return _websocketEmitter.Emit(
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