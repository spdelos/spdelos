using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CSDBPortal.Controllers
{
    public class ConfigurationController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly BaseManager _baseManager;
        private readonly ConfigurationsManager _configurationsManager;

        public ConfigurationController(ApplicationDbContext db, BaseManager baseManager, ConfigurationsManager configurationsManager)
        {
            _db = db;
            _baseManager = baseManager;
            _configurationsManager = configurationsManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _configurationsManager.GetConfigurationDetailInfoAsync());
            }
            catch { }
            return View();
        }

        #region 'IssueNo'
        [HttpPost]
        public async Task<IActionResult> CreateIssueNo()
        {
            try
            {
                int issueNoId = Convert.ToInt32(Request.Form["hdnId"]);
                IssueNo issueNo;
                if (issueNoId <= 0)
                {
                    issueNo = new IssueNo();
                    issueNo.IssueTypeFiles = new List<IssueTypeFile>();
                    issueNo.CreatedBy = User.Identity.Name;
                    issueNo.CreateOn = DateTime.UtcNow;
                }
                else
                {
                    issueNo = await _db.IssueNos.Include(i => i.IssueTypeFiles).FirstOrDefaultAsync(i => i.Id == issueNoId);
                }

                issueNo.Name = Request.Form["txtIssueNo"];

                foreach (IFormFile file in Request.Form.Files)
                {
                    var fileContent = new StringBuilder();
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            fileContent.AppendLine(await reader.ReadLineAsync());
                    }

                    string extension = Path.GetExtension(file.FileName);
                    if (!string.IsNullOrEmpty(extension) && extension.ToLower() == ".xml")
                    {
                        issueNo.BrexTemplate = fileContent.ToString();
                    }
                    else
                    {
                        issueNo.IssueTypeFiles.Add(new IssueTypeFile
                        {
                            Name = file.FileName, CreatedBy = User.Identity.Name,
                            CreateOn = DateTime.UtcNow, Data = fileContent.ToString()
                        });
                    }
                }

                if (issueNoId <= 0)
                    _db.IssueNos.Add(issueNo);

                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Configuration");
            }
            catch
            {
                return View("failed");
            }
        }

        public async Task<JsonResult> DeleteIssueNo(int id)
        {
            var issueNo = await _db.IssueNos.FirstOrDefaultAsync(i => i.Id == id);
            if (issueNo != null) issueNo.IsDelete = true;
            return Json(await _baseManager.CreateOrUpdateRecordAsync(issueNo, "Logical"));
        }
        #endregion

        #region 'Designation'
        public async Task<JsonResult> CreateDesigination(Designation designation)
        {
            string mode;
            if (designation.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                if (await _configurationsManager.CheckDuplicateDesinationAsync(designation) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(designation, mode));
        }

        public async Task<JsonResult> DeleteDesignation(int id)
        {
            var designation = await _db.Designations.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(designation, ""));
        }
        #endregion

        #region 'Info Code Set'
        public async Task<JsonResult> CreateInfoCodeSet(InformationCodeSet informationCodeSet)
        {
            string mode;
            if (informationCodeSet.Id > 0)
            {
                informationCodeSet.UpdatedBy = User.Identity.Name;
                informationCodeSet.UpdatedOn = DateTime.UtcNow;
                mode = "Edit";
            }
            else
            {
                informationCodeSet.CreatedBy = User.Identity.Name;
                informationCodeSet.UpdatedBy = User.Identity.Name;
                informationCodeSet.CreatedOn = DateTime.UtcNow;
                informationCodeSet.UpdatedOn = DateTime.UtcNow;
                if (await _configurationsManager.CheckDuplicateInfoCodeSetAsync(informationCodeSet) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(informationCodeSet, mode));
        }

        public async Task<JsonResult> DeleteInfoCodeSet(int id)
        {
            var informationCodeSet = await _db.InformationCodeSets.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(informationCodeSet, ""));
        }
        #endregion

        #region 'Info Code'
        public async Task<ActionResult> LoadDataModuleType()
        {
            var dbModuleType = await _db.DataModuleTypes.ToListAsync();
            return Json(dbModuleType);
        }

        public async Task<JsonResult> CreateInfoCode(InformationCode informationCode)
        {
            string mode;
            if (informationCode.Id > 0)
            {
                informationCode.UpdatedBy = User.Identity.Name;
                informationCode.UpdatedOn = DateTime.UtcNow;
                mode = "Edit";
            }
            else
            {
                informationCode.CreatedBy = User.Identity.Name;
                informationCode.UpdatedBy = User.Identity.Name;
                informationCode.CreatedOn = DateTime.UtcNow;
                informationCode.UpdatedOn = DateTime.UtcNow;
                if (await _configurationsManager.CheckDuplicateInfoCodeAsync(informationCode) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(informationCode, mode));
        }

        public async Task<JsonResult> DeleteInfoCode(int id)
        {
            var informationCode = await _db.InformationCodes.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(informationCode, ""));
        }
        #endregion

        #region 'ICN Format'
        public async Task<JsonResult> CreateICNFormat(Icnformat icnformat)
        {
            var icnFormatFieldsFromDB = await _db.ICNFormatFields
                .Where(icf => icf.ICNFormatId == icnformat.Id).ToListAsync();

            if (icnformat.Fields != null)
            {
                foreach (ICNFormatField icnFormatField in icnformat.Fields)
                {
                    var fromDB = icnFormatFieldsFromDB.FirstOrDefault(ic => ic.ICNFormatMasterFieldId == icnFormatField.ICNFormatMasterFieldId);
                    if (fromDB != null && fromDB.Id > 0)
                        icnFormatField.Id = fromDB.Id;
                }
            }

            if (icnformat.Id > 0)
            {
                icnformat.UpdatedBy = User.Identity.Name;
                icnformat.UpdatedOn = DateTime.UtcNow;

                if (icnformat.Fields != null)
                {
                    foreach (ICNFormatField field in icnFormatFieldsFromDB)
                    {
                        var formatField = icnformat.Fields.FirstOrDefault(f => f.Id == field.Id);
                        if (formatField == null)
                        {
                            var fromDB = icnFormatFieldsFromDB.FirstOrDefault(ic => ic.Id == field.Id);
                            if (fromDB != null) _db.Entry(fromDB).State = EntityState.Deleted;
                        }
                    }

                    foreach (ICNFormatField icnFormatField in icnformat.Fields)
                    {
                        var fromDB = icnFormatFieldsFromDB.FirstOrDefault(ic => ic.ICNFormatMasterFieldId == icnFormatField.ICNFormatMasterFieldId);
                        if (fromDB != null && fromDB.Id > 0)
                        {
                            fromDB.DisplayOrder = icnFormatField.DisplayOrder;
                            _db.Entry(fromDB).State = EntityState.Modified;
                        }
                        else
                        {
                            icnFormatField.ICNFormatId = icnformat.Id;
                            _db.Add(icnFormatField);
                        }
                    }
                }
                _db.Entry(icnformat).State = EntityState.Modified;
            }
            else
            {
                icnformat.CreatedBy = User.Identity.Name;
                icnformat.UpdatedBy = User.Identity.Name;
                icnformat.CreatedOn = DateTime.UtcNow;
                icnformat.UpdatedOn = DateTime.UtcNow;
                if (await _configurationsManager.CheckDuplicateICTFormatAsync(icnformat) > 0)
                    return Json("Duplicate");
                _db.Icnformats.Add(icnformat);
            }
            await _db.SaveChangesAsync();
            return Json(icnformat);
        }

        public async Task<JsonResult> DeleteICNFormat(int id)
        {
            var informationCode = await _db.Icnformats.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(informationCode, ""));
        }
        #endregion
    }
}
