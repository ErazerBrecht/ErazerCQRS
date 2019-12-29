using System.Collections.Generic;
using Erazer.Read.ViewModels.Ticket.Events;
using Erazer.Web.ReadAPI.ViewModels;

namespace Erazer.Read.ViewModels.Ticket
{
    public class TicketViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }

        public string Description { get; set; }
        public IList<TicketEventViewModel> Events { get; set; }
        public IList<FileViewModel> Files { get; set; }
    }
}