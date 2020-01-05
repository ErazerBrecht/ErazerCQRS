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
    internal class TicketCommentEventHandler : INotificationHandler<TicketCommentDomainEvent>
    {
        private readonly IDbHelper<CommentEventDto> _dbHelper;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IMapper _mapper;

        public TicketCommentEventHandler(IDbHelper<CommentEventDto> dbHelper, IWebsocketEmitter websocketEmitter,
            IMapper mapper)
        {
            _dbHelper = dbHelper ?? throw new ArgumentNullException(nameof(dbHelper));
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
            await _dbHelper.Add(eventDto);
        }

        private Task EmitToFrontEnd(CommentEventDto eventDto)
        {
            return _websocketEmitter.Emit(
                new ReduxCommentAddedAction(_mapper.Map<TicketCommentEventViewModel>(eventDto)));
        }
    }
}