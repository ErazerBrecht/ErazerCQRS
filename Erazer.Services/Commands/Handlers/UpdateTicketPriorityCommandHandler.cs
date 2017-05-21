using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Framework.Domain.Repositories;
using Erazer.Services.Commands.Requests;
using MediatR;

namespace Erazer.Services.Commands.Handlers
{
    public class UpdateTicketPriorityCommandHandler : IAsyncRequestHandler<UpdateTicketPriorityCommand>
    {
        private readonly IAggregateRepository<Ticket> _repository;

        public UpdateTicketPriorityCommandHandler(IAggregateRepository<Ticket> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateTicketPriorityCommand message)
        {
            var ticket = await _repository.Get(message.TicketId);
            ticket.UpdatePriority(message.PriorityId, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
