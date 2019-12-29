namespace Erazer.Web.WriteAPI.ViewModels
{
    public class NewTicketViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PriorityId { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
