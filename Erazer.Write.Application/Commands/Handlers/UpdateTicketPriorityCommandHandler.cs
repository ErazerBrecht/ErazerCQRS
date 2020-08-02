using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using MediatR;

namespace Erazer.Write.Application.Commands.Handlers
{
    internal class UpdateTicketPriorityCommandHandler : AsyncRequestHandler<UpdateTicketPriorityCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketPriorityCommandHandler(IAggregateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(UpdateTicketPriorityCommand message, CancellationToken cancellationToken)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdatePriority(message.PriorityId);
            await _repository.Save(ticket);
        }
    }
}
