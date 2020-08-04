using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Read.Data.Ticket.Detail;
using Erazer.Read.Data.Ticket.Detail.Events;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers.TicketSyncHandlers
{
    public class TicketCommentEventHandler : IEventHandler<TicketCommentPlacedEvent>
    {
        private readonly ISubscriptionContext _ctx;
        private readonly IDbUnitOfWork _db;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IMapper _mapper;

        public TicketCommentEventHandler(ISubscriptionContext ctx, IDbUnitOfWork db, IWebsocketEmitter websocketEmitter,
            IMapper mapper)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Handle(EventEnvelope<TicketCommentPlacedEvent> eventEnvelope,
            CancellationToken cancellationToken)
        {
            var ticketId = eventEnvelope.AggregateRootId.ToString();
            var ticket = await _db.Tickets.Find(ticketId, cancellationToken);

            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                Created = eventEnvelope.Created,
                Comment = eventEnvelope.Event.Comment
            };

            ticket.Events = ticket.Events.Prepend(ticketEvent).ToList();

            await Task.WhenAll(
                UpdateDb(ticket),
                EmitToFrontEnd(ticketId, ticketEvent)
            );
        }

        private Task UpdateDb(TicketDto ticketDto)
        {
            return _db.Tickets.Mutate(ticketDto);
        }

        private Task EmitToFrontEnd(string ticketId, CommentEventDto eventDto)
        {
            if (_ctx.SubscriptionType == SubscriptionType.ReSync)
                return Task.CompletedTask;

            return _websocketEmitter.Emit(
                new ReduxCommentAddedAction(ticketId, _mapper.Map<TicketCommentEventViewModel>(eventDto)));
        }
    }
}