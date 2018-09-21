using System;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Data.DTOs.Events;
using Erazer.Domain.Data.Repositories;
using Erazer.Domain.Events;
using Erazer.Framework.FrontEnd;
using Erazer.Infrastructure.MongoDb;
using Erazer.Web.ReadAPI.DomainEvents.EventHandlers.Redux;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.DomainEvents.EventHandlers
{
    public class TicketCommentEventHandler : AsyncNotificationHandler<TicketCommentDomainEvent>
    {
        private readonly IMongoDbSession _session;
        private readonly IWebsocketEmittor _websocketEmittor;
        private readonly IMapper _mapper;
        private readonly ITicketEventQueryRepository _repository;

        public TicketCommentEventHandler(IWebsocketEmittor websocketEmittor, IMapper mapper, ITicketEventQueryRepository repository, IMongoDbSession session)
        {
            _websocketEmittor = websocketEmittor ?? throw new ArgumentNullException(nameof(websocketEmittor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _session = session;
        }

        protected override async Task HandleCore(TicketCommentDomainEvent message)
        {
            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                Comment = message.Comment
            };

            await _repository.Add(ticketEvent);

            _session.AddSideEffect(() =>
                _websocketEmittor.Emit(new ReduxCommentAddedAction(_mapper.Map<TicketCommentEventViewModel>(ticketEvent)))
            );
        }
    }
}
