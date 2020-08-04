using System;
using System.Threading.Tasks;
using Erazer.Read.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Read.Web.Controllers
{
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Retrieve all tickets
        /// </summary>
        /// <example>
        /// GET api/ticket
        /// </example>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tickets = await _mediator.Send(new TicketListQuery());
            return Ok(tickets);
        }

        /// <summary>
        /// Retrieve detailed information about a specific ticket
        /// </summary>
        /// <example>
        /// GET api/ticket/5
        /// </example>
        /// <param name="id">Ticket id (GUID)</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var ticket = await _mediator.Send(new TicketQuery(id));
            return Ok(ticket);
        }
    }
}

