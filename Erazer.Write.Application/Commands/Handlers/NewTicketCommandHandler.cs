using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using MediatR;

namespace Erazer.Write.Application.Commands.Handlers
{
    internal class NewTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
    {
        private readonly IAggregateRepository _repository;

        public NewTicketCommandHandler(IAggregateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = new Ticket(request.Title, request.Description, request.PriorityId, request.Files);
            await _repository.Save(ticket);

            return ticket.Id;
        }
    }
}
