using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Messages.IntegrationEvents.Models;
using Erazer.Read.Data.File;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers
{
    public class TicketCreateEventHandler : IEventHandler<TicketCreatedEvent>
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

        public async Task Handle(EventEnvelope<TicketCreatedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var priority = await _ctx.Priorities.Find(eventEnvelope.Event.PriorityId, cancellationToken);
            var status = await _ctx.Statuses.Find(eventEnvelope.Event.StatusId, cancellationToken);
            
            var ticketList = new TicketListDto
            {
                Id = eventEnvelope.AggregateRootId.ToString(),
                Priority = priority,
                Status = status,
                Title = eventEnvelope.Event.Title,
                FileCount = eventEnvelope.Event.Files?.Count ?? 0
            };

            var ticket = new TicketDto
            {
                Id = eventEnvelope.AggregateRootId.ToString(),
                Description = eventEnvelope.Event.Description,
                Title = eventEnvelope.Event.Title,
                Priority = priority,
                Status = status,
                Files = eventEnvelope.Event.Files?.Select(f => new FileDto
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
                TicketId = eventEnvelope.AggregateRootId.ToString(),
                Created = eventEnvelope.Created,
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

        private Task PublishOnBus(IEventEnvelope<TicketCreatedEvent> eventEnvelope, PriorityDto priorityDto, StatusDto statusDto,
            TicketEventDto eventDto)
        {
            var files = eventEnvelope.Event.Files.Select(f => new TicketCreatedFile(f.Id, f.Name, f.Type, f.Size));
            var integrationEvent = new TicketCreatedIntegrationEvent(
                eventEnvelope.AggregateRootId.ToString(),
                eventEnvelope.Event.Title,
                eventEnvelope.Event.Description,
                priorityDto.Id,
                priorityDto.Name,
                statusDto.Id,
                statusDto.Name,
                eventDto.Id,
                eventDto.Created,
                files);

            return _eventPublisher.Publish(integrationEvent);
        }
    }
}