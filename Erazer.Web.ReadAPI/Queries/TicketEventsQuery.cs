using System.Collections.Generic;
using MediatR;
using Erazer.Web.ReadAPI.ViewModels.Events;

namespace Erazer.Web.ReadAPI.Queries.Requests
{
    public class TicketEventsQuery : IRequest<List<TicketEventViewModel>>
    {
        public string TicketId { get; set; }
    }
}
