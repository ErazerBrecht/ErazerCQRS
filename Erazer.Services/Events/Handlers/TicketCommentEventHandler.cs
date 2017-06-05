using System;
using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.DTOs.Events;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCommentEventHandler : IAsyncNotificationHandler<TicketCommentEvent>
    {
        private readonly ITicketEventQueryRepository _repository;

        public TicketCommentEventHandler(ITicketEventQueryRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(TicketCommentEvent message)
        {
            var ticketEvent = new TicketEventDto
            {
                Id = Guid.NewGuid().ToString(),
                TicketId = message.AggregateRootId.ToString(),
                Created = message.Created,
                UserId = message.UserId.ToString(),
                Event = new CommentEventDto
                {
                    Comment = message.Comment
                }
            };

            return _repository.Add(ticketEvent);
        }
    }
}
