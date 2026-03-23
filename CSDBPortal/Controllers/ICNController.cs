using CSDBPortal.Business;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class ICNController : BaseController
    {
        private readonly ICNManager _iCNManager;
        private readonly BaseManager _baseManager;

        public ICNController(ICNManager iCNManager, BaseManager baseManager)
        {
            _iCNManager = iCNManager;
            _baseManager = baseManager;
        }

        public IActionResult Index() => View();

        public async Task<JsonResult> GetSequenceNumber(int projectId)
            => Json(await _iCNManager.GetSequenceNumbersAsync(projectId));

        public async Task<JsonResult> GetVarcodes(int projectId, int sequenceNumber)
            => Json(await _iCNManager.GetVarcodesAsync(projectId, sequenceNumber));

        public async Task<JsonResult> GetProjects()
        {
            try
            {
                var result = await _iCNManager.GetProjectsAsync();
                return Json(result.ProjectList);
            }
            catch { }
            return Json(null);
        }

        public async Task<JsonResult> ICNNumberByProjectId(int projectId)
            => Json(await _iCNManager.ICNNumberByProjectIdAsync(projectId));

        public async Task<JsonResult> CreateICNNumber(ICNumberGenerationModel icnNumberGenerationModel)
        {
            await _iCNManager.GenerateICNNumberAsync(icnNumberGenerationModel, User.Identity.Name);
            return Json(true);
        }
    }
}
