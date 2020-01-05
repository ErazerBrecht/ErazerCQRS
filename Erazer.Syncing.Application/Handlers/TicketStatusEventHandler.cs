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
    internal class TicketStatusEventHandler : INotificationHandler<TicketStatusDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly IDbHelper<TicketListDto> _ticketListDbHelper;
        private readonly IDbHelper<TicketDto> _ticketDbHelper;
        private readonly IDbHelper<StatusDto> _statusDbHelper;
        private readonly IDbHelper<StatusEventDto> _statusEventDbHelper;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketStatusEventHandler(IDbHelper<TicketListDto> ticketListDbHelper,
            IDbHelper<TicketDto> ticketDbHelper, IDbHelper<StatusDto> statusDbHelper,
            IDbHelper<StatusEventDto> statusEventDbHelper, IMapper mapper,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ticketListDbHelper = ticketListDbHelper ?? throw new ArgumentNullException(nameof(ticketListDbHelper));
            _ticketDbHelper = ticketDbHelper ?? throw new ArgumentNullException(nameof(ticketDbHelper));
            _statusDbHelper = statusDbHelper ?? throw new ArgumentNullException(nameof(statusDbHelper));
            _statusEventDbHelper = statusEventDbHelper ?? throw new ArgumentNullException(nameof(statusEventDbHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketStatusDomainEvent message, CancellationToken cancellationToken)
        {
            var ticketList = await _ticketListDbHelper.Find(message.AggregateRootId.ToString(), cancellationToken);
            var ticket = await _ticketDbHelper.Find(message.AggregateRootId.ToString(), cancellationToken);
            var oldStatus = await _statusDbHelper.Find(message.FromStatusId, cancellationToken);
            var newStatus = await _statusDbHelper.Find(message.ToStatusId, cancellationToken);

            ticketList.Status = newStatus;
            ticket.Status = newStatus;
            
            // Create ticket status event
            var ticketEvent = new StatusEventDto(oldStatus, newStatus)
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

        private async Task UpdateDb(TicketListDto ticketListDto, TicketDto ticketDto, StatusEventDto eventDto)
        {
            await _ticketListDbHelper.Mutate(ticketListDto);
            await _ticketDbHelper.Mutate(ticketDto);
            await _statusEventDbHelper.Add(eventDto);
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
                eventDto.Created,
                eventDto.UserId
            );

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}