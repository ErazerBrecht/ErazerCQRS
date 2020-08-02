using System;
using Erazer.Messages.Commands;

namespace Erazer.Write.Application.Commands
{
    public class UpdateTicketStatusCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public string StatusId { get; set; }
    }
}
