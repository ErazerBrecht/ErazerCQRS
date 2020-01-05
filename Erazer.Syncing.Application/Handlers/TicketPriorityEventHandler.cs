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
    internal class TicketPriorityEventHandler : INotificationHandler<TicketPriorityDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly IDbHelper<TicketListDto> _ticketListDbHelper;
        private readonly IDbHelper<TicketDto> _ticketDbHelper;
        private readonly IDbHelper<PriorityDto> _priorityDbHelper;
        private readonly IDbHelper<PriorityEventDto> _priorityEventDbHelper;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketPriorityEventHandler(IDbHelper<TicketListDto> ticketListDbHelper,
            IDbHelper<TicketDto> ticketDbHelper, IDbHelper<PriorityDto> priorityDbHelper,
            IDbHelper<PriorityEventDto> priorityEventDbHelper, IMapper mapper,
            IWebsocketEmitter websocketEmitter, IIntegrationEventPublisher eventPublisher)
        {
            _ticketListDbHelper = ticketListDbHelper ?? throw new ArgumentNullException(nameof(ticketListDbHelper));
            _ticketDbHelper = ticketDbHelper ?? throw new ArgumentNullException(nameof(ticketDbHelper));
            _priorityDbHelper = priorityDbHelper ?? throw new ArgumentNullException(nameof(priorityDbHelper));
            _priorityEventDbHelper = priorityEventDbHelper ?? throw new ArgumentNullException(nameof(priorityEventDbHelper));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketPriorityDomainEvent message, CancellationToken cancellationToken)
        {
            var ticketList = await _ticketListDbHelper.Find(message.AggregateRootId.ToString(), cancellationToken);
            var ticket = await _ticketDbHelper.Find(message.AggregateRootId.ToString(), cancellationToken);
            var oldPriority = await _priorityDbHelper.Find(message.FromPriorityId, cancellationToken);
            var newPriority = await _priorityDbHelper.Find(message.ToPriorityId, cancellationToken);

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
            await _ticketListDbHelper.Mutate(ticketListDto);
            await _ticketDbHelper.Mutate(ticketDto);
            await _priorityEventDbHelper.Add(eventDto);
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