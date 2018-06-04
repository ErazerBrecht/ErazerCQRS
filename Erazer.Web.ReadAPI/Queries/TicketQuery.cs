using Erazer.Web.ReadAPI.ViewModels;
using MediatR;

namespace Erazer.Web.ReadAPI.Queries
{
    public class TicketQuery : IRequest<TicketViewModel>
    {
        public string Id { get; set; }
    }
}
