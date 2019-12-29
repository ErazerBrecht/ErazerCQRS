using System;
using Erazer.Read.ViewModels.Ticket;
using MediatR;

namespace Erazer.Read.Application.Queries
{
    public class TicketQuery : IRequest<TicketViewModel>
    {
        public Guid Id { get; }
        
        public TicketQuery(Guid id)
        {
            Id = id;
        }

    }
}
