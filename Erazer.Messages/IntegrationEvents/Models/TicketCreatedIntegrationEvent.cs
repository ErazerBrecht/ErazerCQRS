using System;
using System.Collections.Generic;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace Erazer.Messages.IntegrationEvents.Models
{
    public class TicketCreatedIntegrationEvent : IIntegrationEvent
    {
        public string Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public string PriorityId { get; private set;}
        public string PriorityName { get; private set;}

        public string StatusId { get; private set;}
        public string StatusName { get; private set;}

        public string CreateEventId { get; private set;}
        public long Created { get; private set;}

        public IEnumerable<TicketCreatedFile> Files { get; private set; }

        private TicketCreatedIntegrationEvent()
        {
        }
        
        public TicketCreatedIntegrationEvent(string id, string title, string description, string priorityId, string priorityName,
            string statusId, string statusName, string createEventId, long created, IEnumerable<TicketCreatedFile> files)
        {
            Id = id;
            Title = title;
            Description = description;
            PriorityId = priorityId;
            PriorityName = priorityName;
            StatusId = statusId;
            StatusName = statusName;
            CreateEventId = createEventId;
            Created = created;

            Files = files;
        }
    }

    public class TicketCreatedFile
    {
        public Guid Id { get; private set;}
        public string Name { get; private set;}
        public string Type { get; private set;}
        public int Size { get; private set;}

        private TicketCreatedFile()
        {
        }
        
        public TicketCreatedFile(Guid id, string name, string type, int size)
        {
            Id = id;
            Name = name;
            Type = type;
            Size = size;
        }
    }
}
