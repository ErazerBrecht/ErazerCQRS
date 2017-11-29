using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Services.Events.Redux;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Erazer.Services.Queries.ViewModels;
using MediatR;
using Erazer.Services.Queries.DTOs.Events;
using System;
using Erazer.Services.Queries.ViewModels.Events;
using System.Collections.Generic;

namespace Erazer.Services.Events.Handlers
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
            _ticketRepository = ticketRepository;
            _priorityRepository = priorityRepository;
            _statusRepository = statusRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _websocketEmittor = websocketEmittor;
        }

        public async Task Handle(TicketCreateEvent message)
        {
            var priority = await _priorityRepository.Find(message.PriorityId);
            var status = await _statusRepository.Find(message.StatusId);

            var ticket = new TicketDto
            {
                Id = message.AggregateRootId.ToString(),
                Description = message.Description,
                Title = message.Title,
                Priority = priority,
                Status = status
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

            return _websocketEmittor.Emit(new ReduxAction(ReduxActionTypeConstants.AddTicket, vm));
        }
    }
}
