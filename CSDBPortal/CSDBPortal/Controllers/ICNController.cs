using CSDBPortal.Business;
using CSDBPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class ICNController : BaseController
    {
        ICNManager _iCNManager = new();
        BaseManager _baseManager = new();
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetProjects()
        {
            try
            {
              
                var result = _iCNManager.GetProjects();
                return Json(result.ProjectList);
            }
            catch (Exception ex)
            {
                //
            }
            return Json(null);
        }

        public JsonResult ICNNumberByProjectId(int projectId)
        {
            return Json(_iCNManager.ICNNumberByProjectId(projectId));
        }


        public JsonResult CreateICNNumber(IcnNumber icnNumber)
        {
            icnNumber.DataModuleId = 0;
            icnNumber.IsAllocated = false;
            icnNumber.UpdatedOn = DateTime.UtcNow;
            icnNumber.UpdatedBy = "test@gmail.com";
            return Json(_baseManager.CreateOrUpdateRecord(icnNumber,"Add"));
        }       
    }
}
