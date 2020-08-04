using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Messages.IntegrationEvents.Models;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.Data.Ticket.Detail.Events;
using Erazer.Read.Data.Ticket.List;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers.TicketSyncHandlers
{
    public class TicketPriorityEventHandler : IEventHandler<TicketPriorityChangedEvent>
    {
        private readonly ISubscriptionContext _ctx;
        private readonly IMapper _mapper;
        private readonly IDbUnitOfWork _db;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketPriorityEventHandler(ISubscriptionContext ctx, IMapper mapper, IDbUnitOfWork db, 
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(EventEnvelope<TicketPriorityChangedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var ticketId = eventEnvelope.AggregateRootId.ToString();
            var ticketList = await _db.TicketList.Find(ticketId, cancellationToken);
            var ticket = await _db.Tickets.Find(ticketId, cancellationToken);
            var oldPriority = await _db.Priorities.Find(eventEnvelope.Event.FromPriorityId, cancellationToken);
            var newPriority = await _db.Priorities.Find(eventEnvelope.Event.ToPriorityId, cancellationToken);

            // Create ticket priority event
            var ticketEvent = new PriorityEventDto(oldPriority, newPriority)
            {
                Id = Guid.NewGuid().ToString(),
                Created = eventEnvelope.Created,
            };
            
            ticketList.Priority = newPriority;
            ticket.Priority = newPriority;
            ticket.Events = ticket.Events.Prepend(ticketEvent).ToList();

            await Task.WhenAll(
                UpdateDb(ticketList, ticket, ticketEvent),
                EmitToFrontEnd(ticketId, ticketEvent),
                AddOnBus(ticket, ticketEvent)
            );
        }

        private async Task UpdateDb(TicketListDto ticketListDto, TicketDto ticketDto, PriorityEventDto eventDto)
        {
            await _db.TicketList.Mutate(ticketListDto);
            await _db.Tickets.Mutate(ticketDto);
        }

        private Task EmitToFrontEnd(string ticketId, PriorityEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
            return _websocketEmitter.Emit(
                new ReduxUpdatePriorityAction(ticketId, _mapper.Map<TicketPriorityEventViewModel>(eventDto)));
        }

        private Task AddOnBus(TicketDto ticketDto, PriorityEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
            var integrationEvent = new TicketPriorityIntegrationEvent(
                ticketDto.Priority.Id,
                ticketDto.Priority.Name,
                ticketDto.Id,
                ticketDto.Title,
                eventDto.Id,
                eventDto.Created
            );

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}