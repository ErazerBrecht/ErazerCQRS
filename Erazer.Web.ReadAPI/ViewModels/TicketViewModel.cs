using Erazer.Web.ReadAPI.ViewModels.Events;
using System.Collections.Generic;

namespace Erazer.Web.ReadAPI.ViewModels
{
    public class TicketViewModel : TicketListViewModel
    {
        public string Description { get; set; }
        public IList<TicketEventViewModel> Events { get; set; }
        public IList<FileViewModel> Files { get; set; }
    }
}
 