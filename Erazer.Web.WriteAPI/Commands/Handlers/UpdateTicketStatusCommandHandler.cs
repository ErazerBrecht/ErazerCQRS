using Erazer.Domain;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using Erazer.Web.WriteAPI.Commands.Requests;

namespace Erazer.Web.WriteAPI.Commands.Handlers
{
    public class UpdateTicketStatusCommandHandler : AsyncRequestHandler<UpdateTicketStatusCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketStatusCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        protected override async Task HandleCore(UpdateTicketStatusCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdateStatus(message.StatusId, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
