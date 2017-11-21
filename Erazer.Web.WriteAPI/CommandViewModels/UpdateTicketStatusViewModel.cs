using System;

namespace Erazer.Web.WriteAPI.CommandViewModels
{
    public class UpdateTicketStatusViewModel
    {
        public Guid TicketId { get; set; }
        public string StatusId { get; set; }
    }
}