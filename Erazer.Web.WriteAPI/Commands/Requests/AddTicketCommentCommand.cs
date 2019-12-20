using System;
using Erazer.Framework.Commands;

namespace Erazer.Web.WriteAPI.Commands.Requests
{
    public class AddTicketCommentCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }

        public string Comment { get; set; }
    }
}
