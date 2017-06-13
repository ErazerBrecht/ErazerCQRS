using System;

namespace Erazer.Web.WriteAPI.CommandViewModels
{
    public class UpdateTicketPriorityViewModel
    {
        public Guid TicketId { get; set; }
        public string PriorityId { get; set; }
    }
}