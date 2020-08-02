using System;
using System.Diagnostics.CodeAnalysis;
using Erazer.Framework.Events;

namespace Erazer.Domain.Ticket.Events
{
    [Event("TicketPriorityChanged")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketPriorityChangedEvent : IEvent
    {
        private TicketPriorityChangedEvent()
        {
        }

        public TicketPriorityChangedEvent(string fromPriorityId, string toPriorityId)
        {
            FromPriorityId = fromPriorityId ?? throw new ArgumentNullException(nameof(fromPriorityId));
            ToPriorityId = toPriorityId ?? throw new ArgumentNullException(nameof(toPriorityId));
        }

        public string FromPriorityId { get; private set; }
        public string ToPriorityId { get; private set; }
    }
}