using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Domain.Events;
using Erazer.Domain.Files.Data.DTOs;
using Erazer.Framework.FrontEnd;
using Erazer.Infrastructure.MongoDb;
using Erazer.Messages.IntegrationEvents.Events;
using Erazer.Messages.IntegrationEvents.Infrastructure;
using Erazer.Web.ReadAPI.DomainEvents.EventHandlers.Redux;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.DomainEvents.EventHandlers
{
    public class TicketCreateEventHandler : AsyncNotificationHandler<TicketCreateDomainEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMongoDbSession _session;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly IIntegrationEventPublisher _eventPublisher;

        public TicketCreateEventHandler(ITicketQueryRepository ticketRepository, ITicketEventQueryRepository eventRepository, IMapper mapper, IWebsocketEmittor websocketEmittor,
            IIntegrationEventPublisher eventPublisher, IPriorityQueryRepository priorityRepository, IStatusQueryRepository statusRepository, IMongoDbSession session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        protected override async Task HandleCore(TicketCreateDomainEvent message)
        {
            var priority = _priorityRepository.Find(message.PriorityId);
            var status = _statusRepository.Find(message.StatusId);

            await Task.WhenAll(priority, status);

            var ticket = new TicketDto
            {
                Id = message.AggregateRootId.ToString(),
                Description = message.Description,
                Title = message.Title,
                Priority = priority.Result,
                Status = status.Result,
                Files = message.Files?.Select(f => new FileDto
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
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
            };

            await AddInDb(ticket, @event);
            _session.AddSideEffect(() => SideEffects(ticket, @event));
        }

        private Task AddInDb(TicketDto ticketDto, TicketEventDto eventDto)
        {
            return Task.WhenAll(
                _ticketRepository.Insert(ticketDto),
                _eventRepository.Add(eventDto));
        }

        private Task SideEffects(TicketDto ticketDto, TicketEventDto eventDto)
        {
            return Task.WhenAll(
                EmitToFrontEnd(ticketDto, eventDto),
                AddOnBus(ticketDto, eventDto)
            );
        }

        private Task EmitToFrontEnd(TicketDto ticketDto, TicketEventDto eventDto)
        {
            var vm = _mapper.Map<TicketViewModel>(ticketDto);
            vm.Events = new List<TicketEventViewModel> { _mapper.Map<TicketCreatedEventViewModel>(eventDto) };
            return _websocketEmittor.Emit(new ReduxTicketCreateAction(vm));
        }

        private Task AddOnBus(TicketDto ticketDto, TicketEventDto eventDto)
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
