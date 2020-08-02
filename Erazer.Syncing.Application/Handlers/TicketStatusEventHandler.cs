using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
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
    public class TicketStatusEventHandler : IEventHandler<TicketStatusChangedEvent>
    {
        private readonly IMapper _mapper;
        private readonly IDbRepository<TicketListDto> _ticketListDb;
        private readonly IDbRepository<TicketDto> _ticketDb;
        private readonly IDbRepository<StatusDto> _statusDb;
        private readonly IDbRepository<StatusEventDto> _statusEventDb;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketStatusEventHandler(IDbRepository<TicketListDto> ticketListDb,
            IDbRepository<TicketDto> ticketDb, IDbRepository<StatusDto> statusDb,
            IDbRepository<StatusEventDto> statusEventDb, IMapper mapper,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ticketListDb = ticketListDb ?? throw new ArgumentNullException(nameof(ticketListDb));
            _ticketDb = ticketDb ?? throw new ArgumentNullException(nameof(ticketDb));
            _statusDb = statusDb ?? throw new ArgumentNullException(nameof(statusDb));
            _statusEventDb = statusEventDb ?? throw new ArgumentNullException(nameof(statusEventDb));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(EventEnvelope<TicketStatusChangedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var ticketList = await _ticketListDb.Find(eventEnvelope.AggregateRootId.ToString(), cancellationToken);
            var ticket = await _ticketDb.Find(eventEnvelope.AggregateRootId.ToString(), cancellationToken);
            var oldStatus = await _statusDb.Find(eventEnvelope.Event.FromStatusId, cancellationToken);
            var newStatus = await _statusDb.Find(eventEnvelope.Event.ToStatusId, cancellationToken);

            ticketList.Status = newStatus;
            ticket.Status = newStatus;
            
            // Create ticket status event
            var ticketEvent = new StatusEventDto(oldStatus, newStatus)
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = eventEnvelope.AggregateRootId.ToString(),
                Created = eventEnvelope.Created,
            };

            await Task.WhenAll(
                UpdateDb(ticketList, ticket, ticketEvent),
                EmitToFrontEnd(ticketEvent),
                AddOnBus(ticket, ticketEvent)
            );
        }

        private async Task UpdateDb(TicketListDto ticketListDto, TicketDto ticketDto, StatusEventDto eventDto)
        {
            await _ticketListDb.Mutate(ticketListDto);
            await _ticketDb.Mutate(ticketDto);
            await _statusEventDb.Add(eventDto);
        }

        private Task EmitToFrontEnd(StatusEventDto eventDto)
        {
            return _websocketEmitter.Emit(
                new ReduxUpdateStatusAction(_mapper.Map<TicketStatusEventViewModel>(eventDto)));
        }

        private Task AddOnBus(TicketDto ticketDto, StatusEventDto eventDto)
        {
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