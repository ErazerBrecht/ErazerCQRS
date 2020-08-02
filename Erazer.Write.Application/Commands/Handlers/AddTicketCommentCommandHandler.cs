using System;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Domain.Ticket;
using Erazer.Framework.Domain;
using MediatR;

namespace Erazer.Write.Application.Commands.Handlers
{
    internal class AddTicketCommentCommandHandler : AsyncRequestHandler<AddTicketCommentCommand>
    {
        private readonly IAggregateRepository _repository;

        public AddTicketCommentCommandHandler(IAggregateRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override async Task Handle(AddTicketCommentCommand message, CancellationToken cancellationToken)
        {
            var ticket = await _repository.Get<Ticket>(message.TicketId);
            ticket.AddComment(message.Comment);
            await _repository.Save(ticket);
        }
    }
}
