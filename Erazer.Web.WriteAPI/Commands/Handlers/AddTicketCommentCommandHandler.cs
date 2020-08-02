using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using Erazer.Web.WriteAPI.Commands.Requests;
using MediatR;

namespace Erazer.Web.WriteAPI.Commands.Handlers
{
    public class AddTicketCommentCommandHandler : AsyncRequestHandler<AddTicketCommentCommand>
    {
        private readonly IAggregateRepository _repository;

        public AddTicketCommentCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        protected override async Task HandleCore(AddTicketCommentCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.AddComment(message.Comment, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
