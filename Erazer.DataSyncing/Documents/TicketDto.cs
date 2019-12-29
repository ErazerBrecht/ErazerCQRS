using Erazer.Domain.Files.Data.DTOs;
using System.Collections.Generic;

namespace Erazer.Domain.Data.DTOs
{
    public class TicketDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public PriorityDto Priority { get; set; }
        public StatusDto Status { get; set; }

        public List<FileDto> Files { get; set; }
    }
}
