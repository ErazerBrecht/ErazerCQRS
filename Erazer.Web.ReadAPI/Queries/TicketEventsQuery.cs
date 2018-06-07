using System.Collections.Generic;
using Erazer.Web.ReadAPI.ViewModels.Events;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries
{
    public class TicketEventsQuery : IRequest<List<TicketEventViewModel>>
    {
        public string TicketId { get; set; }
    }
}
