using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Erazer.Services.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ViewComponents
{
    public class StatusChangeViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public StatusChangeViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var statuses = await _mediator.Send(new StatusAllQuery());
            return View(statuses);
        }
    }
}
