using System;
using MediatR;

namespace Erazer.Services.Commands.Requests
{
    public class AddTicketCommentCommand : IRequest
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public string Comment { get; set; }
    }
}
