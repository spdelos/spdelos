using CSDBPortal.Business;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
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

        public JsonResult GetSequenceNumber(int projectId)
        {
            var result = _iCNManager.GetSequenceNumbers(projectId);
            return Json(result);
        }

        public JsonResult GetVarcodes(int projectId, int sequenceNumber)
        {
            var result = _iCNManager.GetVarcodes(projectId, sequenceNumber);
            return Json(result);
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


        public JsonResult CreateICNNumber(ICNumberGenerationModel icnNumberGenerationModel)
        {
            _iCNManager.GenerateICNNumber(icnNumberGenerationModel, User.Identity.Name);
            return Json(true);
        }       
    }
}
