using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using MediatR;

namespace Erazer.Write.Application.Commands.Handlers
{
    internal class UpdateTicketStatusCommandHandler : AsyncRequestHandler<UpdateTicketStatusCommand>
    {
        private readonly IAggregateRepository _repository;

        public UpdateTicketStatusCommandHandler(IAggregateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(UpdateTicketStatusCommand message, CancellationToken cancellationToken)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.UpdateStatus(message.StatusId);
            await _repository.Save(ticket);
        }
    }
}
