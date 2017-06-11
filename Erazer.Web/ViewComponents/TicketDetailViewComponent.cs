using System.Threading.Tasks;
using Erazer.Services.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ViewComponents
{
    public class TicketDetailViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public TicketDetailViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var ticket = await _mediator.Send(new TicketQuery {Id = id });
            return View(ticket);
        }
    }
}
