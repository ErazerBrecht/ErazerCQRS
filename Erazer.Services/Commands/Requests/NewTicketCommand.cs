using System;
using MediatR;

namespace Erazer.Services.Commands.Requests
{
    public class NewTicketCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }
    }
}
