using System;

namespace Erazer.DAL.ReadModel
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int PriorityId { get; set; }
        public string PriorityName { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

    }
}
