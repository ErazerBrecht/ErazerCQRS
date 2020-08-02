using System;

namespace Erazer.Write.Web.ViewModels
{
    public class UpdateTicketPriorityViewModel
    {
        public Guid TicketId { get; set; }
        public string PriorityId { get; set; }
    }
}