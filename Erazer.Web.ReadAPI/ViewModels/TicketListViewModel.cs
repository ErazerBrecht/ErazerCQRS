using Erazer.Framework.FrontEnd;

namespace Erazer.Web.ReadAPI.ViewModels
{
    public class TicketListViewModel: IViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public PriorityViewModel Priority { get; set; }
        public StatusViewModel Status { get; set; }
    }
}
