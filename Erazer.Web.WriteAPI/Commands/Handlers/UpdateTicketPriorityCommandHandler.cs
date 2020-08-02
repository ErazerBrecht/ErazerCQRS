using Erazer.Domain;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using Erazer.Web.WriteAPI.Commands.Requests;

namespace Erazer.Web.WriteAPI.Commands.Handlers
{
    public class UpdateTicketPriorityCommandHandler : AsyncRequestHandler<UpdateTicketPriorityCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketPriorityCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        protected override async Task HandleCore(UpdateTicketPriorityCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdatePriority(message.PriorityId, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
