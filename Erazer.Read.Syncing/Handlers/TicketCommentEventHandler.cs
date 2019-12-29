using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Events;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork.Redux;
using MediatR;

namespace Erazer.Syncing.Handlers
{
    public class TicketCommentEventHandler : INotificationHandler<TicketCommentDomainEvent>
    {
        private readonly ITicketEventQueryRepository _repository;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IMapper _mapper;

        public TicketCommentEventHandler(ITicketEventQueryRepository repository, IWebsocketEmitter websocketEmitter,
            IMapper mapper)
        {
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Handle(TicketCommentDomainEvent notification, CancellationToken cancellationToken)
        {
            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = notification.AggregateRootId.ToString(),
                Created = notification.Created,
                UserId = notification.UserId.ToString(),
                Comment = notification.Comment
            };

            await Task.WhenAll(
                AddInDb(ticketEvent),
                EmitToFrontEnd(ticketEvent)
            );
        }

        private async Task AddInDb(CommentEventDto eventDto)
        {
            await _repository.Add(eventDto);
        }

        private Task EmitToFrontEnd(CommentEventDto eventDto)
        {
            return _websocketEmitter.Emit(
                new ReduxCommentAddedAction(_mapper.Map<TicketCommentEventViewModel>(eventDto)));
        }
    }
}