using System.Collections.Generic;
using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Requests
{
    public class TicketEventsQuery : IRequest<List<TicketEventViewModel>>
    {
        public string TicketId { get; set; }
        public int Page { get; set; }
    }
}
