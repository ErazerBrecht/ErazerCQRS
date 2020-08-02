using System;
using System.Diagnostics.CodeAnalysis;
using Erazer.Framework.Events;

namespace Erazer.Domain.Ticket.Events
{
    [Event("TicketCommentPlaced")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketCommentPlacedEvent : IEvent
    {
        private TicketCommentPlacedEvent()
        {
        }

        public TicketCommentPlacedEvent(string comment)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public string Comment { get; private set; }
    }
}