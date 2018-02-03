using System;

namespace Erazer.Web.WriteAPI.ViewModels
{
    public class UpdateTicketPriorityViewModel
    {
        public Guid TicketId { get; set; }
        public string PriorityId { get; set; }
    }
}