using System;
using System.Collections.Generic;
using Erazer.Domain.Files;
using Erazer.Messages.Commands;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class NewTicketCommand : ICommand<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }

        public List<File> Files { get; set; }
    }
}
