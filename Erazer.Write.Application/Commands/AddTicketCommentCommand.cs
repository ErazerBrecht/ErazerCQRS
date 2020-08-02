using System;
using Erazer.Messages.Commands;

namespace Erazer.Write.Application.Commands
{
    public class AddTicketCommentCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public string Comment { get; set; }
    }
}
