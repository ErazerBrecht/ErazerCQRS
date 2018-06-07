using System.Threading.Tasks;
using Erazer.Framework.FrontEnd;
using Erazer.Web.ReadAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ReadAPI.Controllers
{
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWebsocketEmittor _websocketEmittor;

        public TicketController(IMediator mediator, IWebsocketEmittor websocketEmittor)
        {
            _mediator = mediator;
            _websocketEmittor = websocketEmittor;
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
        public async Task<IActionResult> Get(string id)
        {
            var ticket = await _mediator.Send(new TicketQuery { Id = id });
            return Ok(ticket);
        }

        // 
        /// <summary>
        /// Retrieve all events (comments, history, ...) from a specific ticket
        /// </summary>
        /// <example>
        /// GET api/ticket/5/events
        /// </example>
        /// <param name="id">Ticket id (GUID)</param>
        /// <returns></returns>
        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetEvents(string id)
        {
            var events = await _mediator.Send(new TicketEventsQuery { TicketId = id, });
            return Ok(events);
        }
    }
}

