using System.Collections.Generic;
using Erazer.Read.ViewModels.File;
using Erazer.Read.ViewModels.Ticket.Events;

namespace Erazer.Read.ViewModels.Ticket
{
    public class TicketViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }

        public string Description { get; set; }
        public List<TicketEventViewModel> Events { get; set; }
        public List<FileViewModel> Files { get; set; }
    }
}