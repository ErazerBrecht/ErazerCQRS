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
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IIntegrationEventPublisher _eventPublisher;


        public TicketStatusEventHandler(IMapper mapper, IWebsocketEmitter websocketEmitter,
            ITicketQueryRepository ticketRepository, IStatusQueryRepository statusRepository,
            ITicketEventQueryRepository eventRepository, IIntegrationEventPublisher eventPublisher)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketStatusDomainEvent message, CancellationToken cancellationToken)
        {
            var ticketTask = _ticketRepository.Find(message.AggregateRootId.ToString());
            var oldStatusTask = _statusRepository.Find(message.FromStatusId);
            var newStatusTask = _statusRepository.Find(message.ToStatusId);

            await Task.WhenAll(ticketTask, oldStatusTask, newStatusTask);

            var ticket = ticketTask.Result;
            var oldStatus = oldStatusTask.Result;
            var newStatus = newStatusTask.Result;

            // Update ticket in ReadModel
            ticket.Status = newStatus;

            // Add ticket event in ReadModel
            var ticketEvent = new StatusEventDto(oldStatus, newStatus)
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

        private async Task UpdateDb(TicketDto ticketDto, StatusEventDto eventDto)
        {
            await _ticketRepository.Update(ticketDto);
            await _eventRepository.Add(eventDto);
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