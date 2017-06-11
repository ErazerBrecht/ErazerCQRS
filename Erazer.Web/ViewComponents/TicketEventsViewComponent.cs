using System.Threading.Tasks;
using Erazer.Services.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ViewComponents
{
    public class TicketEventsViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public TicketEventsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var events = await _mediator.Send(new TicketEventsQuery {TicketId = id, });
            return View(events);
        }
    }
}
