using System;
using Erazer.Domain.Files;
using System.Collections.Generic;
using Erazer.Framework.Commands;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class NewTicketCommand : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }

        public List<File> Files { get; set; }
    }
}
