using System.Collections.Generic;
using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries
{
    public class TicketListQuery : IRequest<List<TicketListViewModel>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
