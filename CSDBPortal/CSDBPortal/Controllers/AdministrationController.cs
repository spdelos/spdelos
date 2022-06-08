using CSDBPortal.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class AdministrationController : BaseController
    {
        public IActionResult Index()
        {
            try
            {
                AdministrationManager _administrationManager = new();
                return View(_administrationManager.GetAdministrationDetailInfo());
            }
            catch (Exception ex)
            {
                //todo
            }
            // todo; need to redirect error page or message
            return View();
        }
    }
}
