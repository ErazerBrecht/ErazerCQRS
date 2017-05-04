using System.Threading.Tasks;
using Erazer.Services.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ViewComponents
{
    public class PriorityChangeViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public PriorityChangeViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var priorities = await _mediator.Send(new PriorityAllQuery());
            return View(priorities);
        }
    }
}
