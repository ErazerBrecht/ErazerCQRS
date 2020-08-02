﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Events;
using Erazer.Framework.Events.Envelope;
using Erazer.Read.Data.Ticket.Events;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.SeedWork.Redux;

namespace Erazer.Syncing.Handlers
{
    public class TicketCommentEventHandler : IEventHandler<TicketCommentPlacedEvent>
    {
        private readonly IDbRepository<CommentEventDto> _db;
        private readonly IWebsocketEmitter _websocketEmitter;
        private readonly IMapper _mapper;

        public TicketCommentEventHandler(IDbRepository<CommentEventDto> db, IWebsocketEmitter websocketEmitter,
            IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _websocketEmitter = websocketEmitter ?? throw new ArgumentNullException(nameof(websocketEmitter));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Handle(EventEnvelope<TicketCommentPlacedEvent> eventEnvelope, CancellationToken cancellationToken)
        {
            var ticketEvent = new CommentEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = eventEnvelope.AggregateRootId.ToString(),
                Created = eventEnvelope.Created,
                Comment = eventEnvelope.Event.Comment
            };

            await Task.WhenAll(
                AddInDb(ticketEvent),
                EmitToFrontEnd(ticketEvent)
            );
        }

        private Task AddInDb(CommentEventDto eventDto)
        {
            return _db.Add(eventDto);
        }

        private Task EmitToFrontEnd(CommentEventDto eventDto)
        {
            return _websocketEmitter.Emit(
                new ReduxCommentAddedAction(_mapper.Map<TicketCommentEventViewModel>(eventDto)));
        }
    }
}