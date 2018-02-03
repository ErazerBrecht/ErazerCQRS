using System;
using MediatR;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class UpdateTicketStatusCommand : IRequest
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public string StatusId { get; set; }
    }
}
