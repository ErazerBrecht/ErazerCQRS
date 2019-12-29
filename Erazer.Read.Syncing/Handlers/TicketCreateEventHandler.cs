using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Messages.IntegrationEvents.Models;
using Erazer.Read.Data.File;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork.Redux;
using MediatR;

namespace Erazer.Syncing.Handlers
{
    public class TicketCreateEventHandler : INotificationHandler<TicketCreateDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketCreateEventHandler(IMapper mapper, IWebsocketEmitter websocketEmitter,
            IIntegrationEventPublisher eventPublisher,
            ITicketQueryRepository ticketRepository, ITicketEventQueryRepository eventRepository,
            IPriorityQueryRepository priorityRepository, IStatusQueryRepository statusRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        public async Task Handle(TicketCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            var priority = _priorityRepository.Find(notification.PriorityId);
            var status = _statusRepository.Find(notification.StatusId);

            await Task.WhenAll(priority, status);

            var ticket = new TicketDto
            {
                Id = notification.AggregateRootId.ToString(),
                Description = notification.Description,
                Title = notification.Title,
                Priority = priority.Result,
                Status = status.Result,
                Files = notification.Files?.Select(f => new FileDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Type = f.Type,
                    Size = f.Size
                }).ToList()
            };

            var @event = new CreatedEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = notification.AggregateRootId.ToString(),
                Created = notification.Created,
                UserId = notification.UserId.ToString(),
            };

            await Task.WhenAll(
                AddInDb(ticket, @event),
                EmitToFrontEnd(ticket, @event),
                PublishOnBus(ticket, @event)
            );
        }

        private async Task AddInDb(TicketDto ticketDto, TicketEventDto eventDto)
        {
            await _ticketRepository.Insert(ticketDto);
            await _eventRepository.Add(eventDto));
        }

        private Task EmitToFrontEnd(TicketDto ticketDto, TicketEventDto eventDto)
        {
            var vm = _mapper.Map<TicketViewModel>(ticketDto);
            vm.Events = new List<TicketEventViewModel> {_mapper.Map<TicketCreatedEventViewModel>(eventDto)};
            return _websocketEmitter.Emit(new ReduxTicketCreateAction(vm));
        }

        private Task PublishOnBus(TicketDto ticketDto, TicketEventDto eventDto)
        {
            var files = ticketDto.Files.Select(f => new TicketCreatedFile(f.Id, f.Name, f.Type, f.Size));
            var integrationEvent = new TicketCreatedIntegrationEvent(
                ticketDto.Id,
                ticketDto.Title,
                ticketDto.Description,
                ticketDto.Priority.Id,
                ticketDto.Priority.Name,
                ticketDto.Status.Id,
                ticketDto.Status.Name,
                eventDto.Id,
                eventDto.Created,
                eventDto.UserId,
                files);

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}