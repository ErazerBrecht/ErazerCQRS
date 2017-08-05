using System.Threading.Tasks;
using Erazer.Domain.Events;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using MediatR;

namespace Erazer.Services.Events.Handlers
{
    public class TicketCreateEventHandler : IAsyncNotificationHandler<TicketCreateEvent>
    {
        private readonly ITicketQueryRepository _repository;

        public TicketCreateEventHandler(ITicketQueryRepository repository)
        {
            _repository = repository;
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
        }
    }
}
