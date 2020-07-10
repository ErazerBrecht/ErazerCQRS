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
        private readonly IDbRepository<TicketListDto> _ticketListDb;
        private readonly IDbRepository<TicketDto> _ticketDb;
        private readonly IDbRepository<PriorityDto> _priorityDb;
        private readonly IDbRepository<PriorityEventDto> _priorityEventDb;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketPriorityEventHandler(IDbRepository<TicketListDto> ticketListDb,
            IDbRepository<TicketDto> ticketDb, IDbRepository<PriorityDto> priorityDb,
            IDbRepository<PriorityEventDto> priorityEventDb, IMapper mapper,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ticketListDb = ticketListDb ?? throw new ArgumentNullException(nameof(ticketListDb));
            _ticketDb = ticketDb ?? throw new ArgumentNullException(nameof(ticketDb));
            _priorityDb = priorityDb ?? throw new ArgumentNullException(nameof(priorityDb));
            _priorityEventDb = priorityEventDb ?? throw new ArgumentNullException(nameof(priorityEventDb));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketPriorityDomainEvent message, CancellationToken cancellationToken)
        {
            var ticketList = await _ticketListDb.Find(message.AggregateRootId.ToString(), cancellationToken);
            var ticket = await _ticketDb.Find(message.AggregateRootId.ToString(), cancellationToken);
            var oldPriority = await _priorityDb.Find(message.FromPriorityId, cancellationToken);
            var newPriority = await _priorityDb.Find(message.ToPriorityId, cancellationToken);

            ticketList.Priority = newPriority;
            ticket.Priority = newPriority;
            
            // Create ticket priority event
            var ticketEvent = new PriorityEventDto(oldPriority, newPriority)
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
            };

            await Task.WhenAll(
                UpdateDb(ticketList, ticket, ticketEvent),
                EmitToFrontEnd(ticketEvent),
                AddOnBus(ticket, ticketEvent)
            );
        }

        private async Task UpdateDb(TicketListDto ticketListDto, TicketDto ticketDto, PriorityEventDto eventDto)
        {
            await _ticketListDb.Mutate(ticketListDto);
            await _ticketDb.Mutate(ticketDto);
            await _priorityEventDb.Add(eventDto);
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