using Erazer.Services.Queries.ViewModels;
using MediatR;

namespace Erazer.Services.Queries.Requests
{
    public class TicketQuery : IRequest<TicketViewModel>
    {
        public string Id { get; set; }
    }
}
