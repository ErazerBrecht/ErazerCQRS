using System;
using Erazer.Framework.Commands;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class UpdateTicketStatusCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public string StatusId { get; set; }
    }
}
