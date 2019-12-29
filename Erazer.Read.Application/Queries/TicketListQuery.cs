using System.Collections.Generic;
using Erazer.Read.ViewModels.Ticket;
using MediatR;

namespace Erazer.Read.Application.Queries
{
    public class TicketListQuery : IRequest<List<TicketListViewModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
