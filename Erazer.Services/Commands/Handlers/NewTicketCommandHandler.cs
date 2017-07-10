using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Framework.Domain;
using Erazer.Services.Commands.Requests;
using MediatR;

namespace Erazer.Services.Commands.Handlers
{
    public class NewTicketCommandHandler : IAsyncRequestHandler<NewTicketCommand>
    {
        private readonly IAggregateRepository _repository;

        public NewTicketCommandHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(NewTicketCommand message)
        {
            var ticket = new Ticket(message.Id, message.Title, message.Description, message.UserId);
            await _repository.Save(ticket);
        }
    }
}
