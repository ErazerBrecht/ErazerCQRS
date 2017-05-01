using System;

namespace Erazer.DAL.ReadModel
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Priority { get; set; }
        public string Status { get; set; }

    }
}
