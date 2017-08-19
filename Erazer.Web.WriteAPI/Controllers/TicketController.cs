using System;
using System.Threading.Tasks;
using Erazer.Services.Commands.Requests;
using Erazer.Web.WriteAPI.CommandViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.WriteAPI.Controllers
{
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] NewTicketViewModel model)
        {
            await _mediator.Send(new NewTicketCommand
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Title = model.Title,
                Description = model.Description,
                PriorityId = model.PriorityId
            });

            return Ok();
        }

        [HttpPost("comment")]
        public async Task<IActionResult> AddComment([FromBody] AddTicketCommentViewModel model)
        {
            await _mediator.Send(new AddTicketCommentCommand
            {
                Comment = model.Comment,
                TicketId = model.TicketId,
                UserId = Guid.Parse("88888888-8888-8888-8888-888888888888")
            });

            return Ok();
        }

        [HttpPut("priority")]
        public async Task<IActionResult> UpdatePriority([FromBody] UpdateTicketPriorityViewModel model)
        {
            await _mediator.Send(new UpdateTicketPriorityCommand
            {
                PriorityId = model.PriorityId,
                TicketId = model.TicketId,
                UserId = Guid.Parse("88888888-8888-8888-8888-888888888888")
            });

            return Ok();
        }
    }
}
