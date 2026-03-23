using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class ViewerController : BaseController
    {
        public IActionResult Index() => View();
    }
}
