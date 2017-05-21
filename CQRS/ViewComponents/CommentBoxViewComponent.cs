using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Erazer.Web.ViewComponents
{
    public class CommentBoxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
           return await Task.FromResult(View(model: id));
        }
    }
}
