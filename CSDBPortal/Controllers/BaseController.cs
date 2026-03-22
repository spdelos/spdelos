using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }
    }
}
