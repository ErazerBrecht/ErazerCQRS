using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;
using AutoMapper;
using Erazer.Services.Events.Redux;
using Erazer.Services.Queries.ViewModels.Events;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCommentEventHandler : IAsyncNotificationHandler<TicketCommentEvent>
    {
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly IMapper _mapper;
        private readonly ITicketEventQueryRepository _repository;

        public TicketCommentEventHandler(IWebsocketEmittor websocketEmittor, IMapper mapper, ITicketEventQueryRepository repository)
        {
            _websocketEmittor = websocketEmittor;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Handle(TicketCommentEvent message)
        {
            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                Comment = message.Comment                
            };

            await Task.WhenAll(
                _websocketEmittor.Emit(new ReduxAction(ReduxActionTypeConstants.AddTicketComment, _mapper.Map<TicketCommentEventViewModel>(ticketEvent))),
                _repository.Add(ticketEvent));
        }
    }
}
