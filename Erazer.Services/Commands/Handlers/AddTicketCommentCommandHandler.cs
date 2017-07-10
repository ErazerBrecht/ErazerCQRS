using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Services.Commands.Requests;
using MediatR;

namespace Erazer.Services.Commands.Handlers
{
    public class AddTicketCommentCommandHandler : IAsyncRequestHandler<AddTicketCommentCommand>
    {
        private readonly IAggregateRepository _repository;

        public AddTicketCommentCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(AddTicketCommentCommand message)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.AddComment(message.Comment, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
