using System;

namespace Erazer.Services.Queries.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int PriorityId { get; set; }
        public string PriorityName { get; }

        public int StatusId { get; set; }
        public string StatusName { get; }

    }
}
