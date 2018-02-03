using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Erazer.Domain.Events;
using Erazer.Web.ReadAPI.ViewModels.Events;
using Erazer.Framework.FrontEnd;
using Erazer.Domain.Infrastructure.Repositories;
using Erazer.Domain.Infrastructure.DTOs.Events;
using Erazer.Web.ReadAPI.EventHandlers.Redux;

namespace Erazer.Web.ReadAPI.EventHandlers
{
    public class TicketCommentEventHandler : IAsyncNotificationHandler<TicketCommentEvent>
    {
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly IMapper _mapper;
        private readonly ITicketEventQueryRepository _repository;

        public TicketCommentEventHandler(IWebsocketEmittor websocketEmittor, IMapper mapper, ITicketEventQueryRepository repository)
        {
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task Handle(TicketCommentEvent message)
        {
            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                Comment = message.Comment                
            };

            return Task.WhenAll(
                _websocketEmittor.Emit(new ReduxCommentAddedAction(_mapper.Map<TicketCommentEventViewModel>(ticketEvent))),
                _repository.Add(ticketEvent));
        }
    }
}
