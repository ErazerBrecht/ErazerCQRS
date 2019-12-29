using System.Collections.Generic;
using Erazer.Read.Data.File;
using Erazer.Read.Data.Ticket.Events;

namespace Erazer.Read.Data.Ticket
{
    public class TicketDto: IProjection
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PriorityDto Priority { get; set; }
        public StatusDto Status { get; set; }

        public List<FileDto> Files { get; set; }
        public List<TicketEventDto> Events { get; set; }
    }
}
