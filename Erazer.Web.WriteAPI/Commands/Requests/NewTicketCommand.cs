using System;
using MediatR;
using Erazer.Domain.Files;
using System.Collections.Generic;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class NewTicketCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }

        public List<File> Files { get; set; }
    }
}
