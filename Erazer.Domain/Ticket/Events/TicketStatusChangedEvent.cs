using System;
using System.Diagnostics.CodeAnalysis;
using Erazer.Framework.Events;

namespace Erazer.Domain.Ticket.Events
{
    [Event("TicketStatusChanged")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketStatusChangedEvent : IEvent
    {
        public TicketStatusChangedEvent(string fromStatusId, string toStatusId)
        {
            FromStatusId = fromStatusId ?? throw new ArgumentNullException(nameof(fromStatusId));
            ToStatusId = toStatusId ?? throw new ArgumentNullException(nameof(toStatusId));
        }

        public string FromStatusId { get; private set; }
        public string ToStatusId { get; private set; }
    }
}