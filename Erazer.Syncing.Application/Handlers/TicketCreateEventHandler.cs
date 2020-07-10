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
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;
        private readonly IDbUnitOfWork _ctx;

        public TicketCreateEventHandler(IMapper mapper, IWebsocketEmitter websocketEmitter,
            IIntegrationEventPublisher eventPublisher, IDbUnitOfWork ctx)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task Handle(TicketCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            var priority = await _ctx.Priorities.Find(notification.PriorityId, cancellationToken);
            var status = await _ctx.Statuses.Find(notification.StatusId, cancellationToken);
            
            var ticketList = new TicketListDto
            {
                Id = notification.AggregateRootId.ToString(),
                Priority = priority,
                Status = status,
                Title = notification.Title,
                FileCount = notification.Files?.Count ?? 0
            };

            var ticket = new TicketDto
            {
                Id = notification.AggregateRootId.ToString(),
                Description = notification.Description,
                Title = notification.Title,
                Priority = priority,
                Status = status,
                Files = notification.Files?.Select(f => new FileDto
                {
                    Id = f.Id.ToString(),
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

            await AddInDb(ticketList, ticket, @event);
            // await Task.WhenAll(
            //     AddInDb(ticketList, ticket, @event),
            //     EmitToFrontEnd(ticket, @event),
            //     PublishOnBus(notification, priority, status, @event)
            // );
        }

        private async Task AddInDb(TicketListDto ticketListDto, TicketDto ticketDto, CreatedEventDto eventDto)
        {
            await _ctx.TicketList.Add(ticketListDto);
            await _ctx.Tickets.Add(ticketDto);
            await _ctx.TicketEvents.Add(eventDto);
        }

        private Task EmitToFrontEnd(TicketDto ticketDto, TicketEventDto eventDto)
        {
            var vm = _mapper.Map<TicketViewModel>(ticketDto);
            vm.Events = new List<TicketEventViewModel> {_mapper.Map<TicketCreatedEventViewModel>(eventDto)};
            return _websocketEmitter.Emit(new ReduxTicketCreateAction(vm));
        }

        private Task PublishOnBus(TicketCreateDomainEvent notification, PriorityDto priorityDto, StatusDto statusDto,
            TicketEventDto eventDto)
        {
            var files = notification.Files.Select(f => new TicketCreatedFile(f.Id, f.Name, f.Type, f.Size));
            var integrationEvent = new TicketCreatedIntegrationEvent(
                notification.AggregateRootId.ToString(),
                notification.Title,
                notification.Description,
                priorityDto.Id,
                priorityDto.Name,
                statusDto.Id,
                statusDto.Name,
                eventDto.Id,
                eventDto.Created,
                eventDto.UserId,
                files);

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}