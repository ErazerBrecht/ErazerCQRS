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
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.Data.Ticket.Detail.Events;
using Erazer.Read.Data.Ticket.List;
using Erazer.Read.ViewModels.Ticket;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers.TicketSyncHandlers
{
    public class TicketCreateEventHandler : IEventHandler<TicketCreatedEvent>
    {
        private readonly ISubscriptionContext _ctx;
        private readonly IMapper _mapper;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IIntegrationEventPublisher _eventPublisher;
        private readonly IDbUnitOfWork _db;

        public TicketCreateEventHandler(ISubscriptionContext ctx, IMapper mapper, IWebsocketEmitter websocketEmitter,
            IIntegrationEventPublisher eventPublisher, IDbUnitOfWork db)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task Handle(EventEnvelope<TicketCreatedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var priority = await _db.Priorities.Find(eventEnvelope.Event.PriorityId, cancellationToken);
            var status = await _db.Statuses.Find(eventEnvelope.Event.StatusId, cancellationToken);
            
            var ticketList = new TicketListDto
            {
                Id = eventEnvelope.AggregateRootId.ToString(),
                Priority = priority,
                Status = status,
                Title = eventEnvelope.Event.Title,
                FileCount = eventEnvelope.Event.Files?.Count ?? 0
            };

            var ticketEvent = new CreatedEventDto
            {
                Id = Guid.NewGuid().ToString(),
                Created = eventEnvelope.Created,
            };
            
            var ticket = new TicketDto
            {
                Id = eventEnvelope.AggregateRootId.ToString(),
                Description = eventEnvelope.Event.Description,
                Title = eventEnvelope.Event.Title,
                Priority = priority,
                Status = status,
                Events = new List<TicketEventDto> { ticketEvent },
                Files = eventEnvelope.Event.Files?.Select(f => new FileDto
                {
                    Id = f.Id.ToString(),
                    Name = f.Name,
                    Type = f.Type,
                    Size = f.Size
                }).ToList()
            };

            await Task.WhenAll(
                AddInDb(ticketList, ticket),
                EmitToFrontEnd(ticket),
                PublishOnBus(eventEnvelope, priority, status, ticketEvent)
            );
        }

        private async Task AddInDb(TicketListDto ticketListDto, TicketDto ticketDto)
        {
            await _db.TicketList.Add(ticketListDto);
            await _db.Tickets.Add(ticketDto);
        }

        private Task EmitToFrontEnd(TicketDto ticketDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
            var vm = _mapper.Map<TicketViewModel>(ticketDto);
            return _websocketEmitter.Emit(new ReduxTicketCreateAction(vm));
        }

        private Task PublishOnBus(IEventEnvelope<TicketCreatedEvent> eventEnvelope, PriorityDto priorityDto, StatusDto statusDto,
            TicketEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;
            
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