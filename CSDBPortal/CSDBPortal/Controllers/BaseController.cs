using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
            //if (User != null && User.Identity.IsAuthenticated)
            //{
            //    Response.Redirect("~/Administration");
            //}
            //else
            //{
            //    Response.Redirect("~/Identity/Account/Login");
            //}
        }
    }
}
