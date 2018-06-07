using System;
using Erazer.Framework.Commands;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class UpdateTicketPriorityCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public string PriorityId { get; set; }
    }
}
