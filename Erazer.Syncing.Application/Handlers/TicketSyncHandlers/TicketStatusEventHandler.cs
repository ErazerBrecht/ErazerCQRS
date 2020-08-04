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
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers.TicketSyncHandlers
{
    public class TicketStatusEventHandler : IEventHandler<TicketStatusChangedEvent>
    {
        private readonly ISubscriptionContext _ctx;
        private readonly IMapper _mapper;
        private readonly IDbUnitOfWork _db;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketStatusEventHandler(ISubscriptionContext ctx, IMapper mapper, IDbUnitOfWork db,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(EventEnvelope<TicketStatusChangedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var ticketId = eventEnvelope.AggregateRootId.ToString();
            var ticketList = await _db.TicketList.Find(ticketId, cancellationToken);
            var ticket = await _db.Tickets.Find(ticketId, cancellationToken);
            var oldStatus = await _db.Statuses.Find(eventEnvelope.Event.FromStatusId, cancellationToken);
            var newStatus = await _db.Statuses.Find(eventEnvelope.Event.ToStatusId, cancellationToken);

            // Create ticket status event
            var ticketEvent = new StatusEventDto(oldStatus, newStatus)
            {
                Id = Guid.NewGuid().ToString(),
                Created = eventEnvelope.Created,
            };
            
            ticketList.Status = newStatus;
            ticket.Status = newStatus;
            ticket.Events = ticket.Events.Prepend(ticketEvent).ToList();
            
            await Task.WhenAll(
                UpdateDb(ticketList, ticket),
                EmitToFrontEnd(ticketId, ticketEvent),
                AddOnBus(ticket, ticketEvent)
            );
        }

        private async Task UpdateDb(TicketListDto ticketListDto, TicketDto ticketDto)
        {
            await _db.TicketList.Mutate(ticketListDto);
            await _db.Tickets.Mutate(ticketDto);
        }

        private Task EmitToFrontEnd(string ticketId, StatusEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
            return _websocketEmitter.Emit(
                new ReduxUpdateStatusAction(ticketId, _mapper.Map<TicketStatusEventViewModel>(eventDto)));
        }

        private Task AddOnBus(TicketDto ticketDto, StatusEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
            var integrationEvent = new TicketStatusIntegrationEvent(
                ticketDto.Status.Id,
                ticketDto.Status.Name,
                ticketDto.Id,
                ticketDto.Title,
                eventDto.Id,
                eventDto.Created
            );

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}