using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Services.Events.Mappings;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCreateEventHandler : IAsyncNotificationHandler<TicketCreateEvent>
    {
        private readonly IMapper _mapper;
        private readonly ITicketQueryRepository _repository;
        private readonly IWebsocketEmittor _websocketEmittor;

        public TicketCreateEventHandler(ITicketQueryRepository repository, IMapper mapper, IWebsocketEmittor websocketEmittor)
        {
            _repository = repository;
            _mapper = mapper;
            _websocketEmittor = websocketEmittor;
        }

        public async Task Handle(TicketCreateEvent message)
        {
            var ticket = new TicketDto
            {
                Id = message.AggregateRootId.ToString(),
                Description = message.Description,
                Title = message.Title
            };

            await _repository.Insert(ticket);
            await _websocketEmittor.Emit(_mapper.Map<ReduxAction>(message));
        }
    }
}
