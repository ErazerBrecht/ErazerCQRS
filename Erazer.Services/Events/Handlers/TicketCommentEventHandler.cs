using System;
using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Domain.Aggregates.Repositories;
using Erazer.Domain.Events;
using Erazer.Services.Commands.Requests;
using Erazer.Services.Events.Entities;
using Erazer.Services.Events.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCommentEventHandler : IAsyncRequestHandler<TicketCommentEvent>
    {
        private readonly ITicketCommentEventRepository _repository;

        public TicketCommentEventHandler(ITicketCommentEventRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(TicketCommentEvent message)
        {
            // AutoMapper!?
            var eventEntity = new TicketCommentEventEntity
            {
                Id = Guid.NewGuid(),
                TicketId = message.AggregateRootId,
                Created = message.Created,
                UserId = message.UserId,
                Comment = message.Comment
            };

            _repository.Add(eventEntity);
            await _repository.Commit();
        }
    }
}
