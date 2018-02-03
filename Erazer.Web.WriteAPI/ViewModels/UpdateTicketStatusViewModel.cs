using System;

namespace Erazer.Web.WriteAPI.ViewModels
{
    public class UpdateTicketStatusViewModel
    {
        public Guid TicketId { get; set; }
        public string StatusId { get; set; }
    }
}