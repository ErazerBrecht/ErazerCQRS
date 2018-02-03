using System.Collections.Generic;
using MediatR;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Web.ReadAPI.Queries.Requests
{
    public class TicketListQuery : IRequest<List<TicketListViewModel>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
