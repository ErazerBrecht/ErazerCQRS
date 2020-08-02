using System;
using Erazer.Messages.Commands;

namespace Erazer.Write.Application.Commands
{
    public class UpdateTicketPriorityCommand : ICommand
    {
        public Guid TicketId { get; set; }
        public string PriorityId { get; set; }
    }
}
