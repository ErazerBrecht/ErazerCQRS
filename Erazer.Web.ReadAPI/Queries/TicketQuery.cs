using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries.Requests
{
    public class TicketQuery : IRequest<TicketViewModel>
    {
        public string Id { get; set; }
    }
}
