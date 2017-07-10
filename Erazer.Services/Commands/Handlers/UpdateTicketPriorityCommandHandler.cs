using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Services.Commands.Requests;
using MediatR;

namespace Erazer.Services.Commands.Handlers
{
    public class UpdateTicketPriorityCommandHandler : IAsyncRequestHandler<UpdateTicketPriorityCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketPriorityCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateTicketPriorityCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdatePriority(message.PriorityId, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
