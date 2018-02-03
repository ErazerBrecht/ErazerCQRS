using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Web.WriteAPI.Commands.Requests;
using MediatR;

namespace Erazer.Web.WriteAPI.Commands.Handlers
{
    public class UpdateTicketStatusCommandHandler : IAsyncRequestHandler<UpdateTicketStatusCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketStatusCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateTicketStatusCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdateStatus(message.StatusId, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
