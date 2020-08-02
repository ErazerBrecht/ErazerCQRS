using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Domain;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using Erazer.Web.WriteAPI.Commands.Requests;
using MediatR;

namespace Erazer.Web.WriteAPI.Commands.Handlers
{
    public class NewTicketCommandHandler : IRequestHandler<NewTicketCommand, Guid>
    {
        private readonly IAggregateRepository _repository;

        public NewTicketCommandHandler(IAggregateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<Guid> Handle(NewTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket(request.Title, request.Description, request.PriorityId, request.Files);
            await _repository.Save(ticket);

            return ticket.Id;
        }
    }
}
