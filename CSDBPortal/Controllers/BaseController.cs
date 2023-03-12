using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        IServiceProvider serviceProvider;
        private readonly IWebHostEnvironment appEnvironment;
        public BaseController()
        {
        }
    }
}
