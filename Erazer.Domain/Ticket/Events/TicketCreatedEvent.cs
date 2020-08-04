using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Erazer.Domain.Files;
using Erazer.Framework.Events;

namespace Erazer.Domain.Ticket.Events
{
    [Event("TicketCreated")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TicketCreatedEvent : IEvent
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string PriorityId { get; private set; }
        public string StatusId { get; private set; }
        public List<File> Files { get; private set; }
        
        private TicketCreatedEvent()
        {
        }

        public TicketCreatedEvent(string title, string description, string priorityId, string statusId,
            List<File> files)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            PriorityId = priorityId ?? throw new ArgumentNullException(nameof(priorityId));
            StatusId = statusId ?? throw new ArgumentNullException(nameof(statusId));
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }
    }
}