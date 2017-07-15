using System.Collections.Generic;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Requests
{
    public class TicketListQuery : IRequest<List<TicketListViewModel>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
