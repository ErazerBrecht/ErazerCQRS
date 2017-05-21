using System;
using MediatR;

namespace Erazer.Services.Commands.Requests
{
    public class UpdateTicketPriorityCommand : IRequest
    {
        public string TicketId { get; set; }
        public Guid UserId { get; set; }

        public int PriorityId { get; set; }
    }
}
