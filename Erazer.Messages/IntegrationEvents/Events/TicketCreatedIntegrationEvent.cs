using System;
using System.Collections.Generic;
namespace Erazer.Messages.IntegrationEvents.Events
{
    public class TicketCreatedIntegrationEvent : IIntegrationEvent
    {
        public string Id { get; }
        public string Title { get; }
        public string Description { get; }

        public string PriorityId { get; }
        public string PriorityName { get; }

        public string StatusId { get; }
        public string StatusName { get; }

        public string CreateEventId { get; }
        public DateTime Created { get; }
        public string UserId { get; }

        public IEnumerable<TicketCreatedFile> Files { get; set; }

        public TicketCreatedIntegrationEvent(string id, string title, string description, string priorityId, string priorityName,
            string statusId, string statusName, string createEventId, DateTime created, string userId, IEnumerable<TicketCreatedFile> files)
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
            UserId = userId;

            Files = files;
        }
    }

    public class TicketCreatedFile
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Type { get; }
        public int Size { get; }

        public TicketCreatedFile(Guid id, string name, string type, int size)
        {
            Id = id;
            Name = name;
            Type = type;
            Size = size;
        }
    }
}
