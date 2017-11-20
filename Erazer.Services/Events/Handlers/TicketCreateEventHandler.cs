using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Services.Events.Redux;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCreateEventHandler : IAsyncNotificationHandler<TicketCreateEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _ticketRepository;
        private readonly IPriorityQueryRepository _priorityRepository;
        private readonly IStatusQueryRepository _statusRepository;
        private readonly IWebsocketEmittor _websocketEmittor;

        public TicketCreateEventHandler(ITicketQueryRepository ticketRepository, IMapper mapper, IWebsocketEmittor websocketEmittor, IPriorityQueryRepository priorityRepository, IStatusQueryRepository statusRepository)
        {
            _ticketRepository = ticketRepository;
            _priorityRepository = priorityRepository;
            _statusRepository = statusRepository;
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

            await Task.WhenAll(
                    _websocketEmittor.Emit(new ReduxAction(ReduxActionTypeConstants.AddTicket, _mapper.Map<TicketListViewModel>(ticket))),
                    _ticketRepository.Insert(ticket));
        }
    }
}
