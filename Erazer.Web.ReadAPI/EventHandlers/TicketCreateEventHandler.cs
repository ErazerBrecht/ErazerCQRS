using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Erazer.Domain.Events;
using Erazer.Web.ReadAPI.ViewModels;
using Erazer.Web.ReadAPI.ViewModels.Events;
using Erazer.Framework.FrontEnd;
using Erazer.Domain.Data.DTOs;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Web.ReadAPI.EventHandlers.Redux;
using System.Linq;
using Erazer.Domain.Files.Data.DTOs;

namespace Erazer.Web.ReadAPI.EventHandlers
{
    public class TicketCreateEventHandler : IAsyncNotificationHandler<TicketCreateEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly ITicketEventQueryRepository _eventRepository;
        private readonly IWebsocketEmittor _websocketEmittor;

        public TicketCreateEventHandler(ITicketQueryRepository ticketRepository, ITicketEventQueryRepository eventRepository, IMapper mapper, IWebsocketEmittor websocketEmittor, IPriorityQueryRepository priorityRepository, IStatusQueryRepository statusRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _priorityRepository = priorityRepository ?? throw new ArgumentNullException(nameof(priorityRepository));
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
        }

        public async Task Handle(TicketCreateEvent message)
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
                    Id = f.Id.ToString(),
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

            await Task.WhenAll(AddInDb(ticket, @event), EmitToFrontEnd(ticket, @event));
        }

        private Task AddInDb(TicketDto ticketDto, TicketEventDto eventDto)
        {
            return Task.WhenAll(
                _ticketRepository.Insert(ticketDto),
                _eventRepository.Add(eventDto));
        }

        private Task EmitToFrontEnd(TicketDto ticketDto, TicketEventDto eventDto)
        {
            var vm = _mapper.Map<TicketViewModel>(ticketDto);
            vm.Events = new List<TicketEventViewModel> { _mapper.Map<TicketCreatedEventViewModel>(eventDto) };

            return _websocketEmittor.Emit(new ReduxTicketCreateAction(vm));
        }
    }
}
