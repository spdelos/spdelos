using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Text;
using System.Xml;

namespace CSDBPortal.Controllers
{
    public class ConfigurationController : BaseController
    {
        BaseManager _baseManager = new();
        ConfigurationsManager _configurationsManager = new();

        //[Authorize]
        public IActionResult Index()
        {
            try
            {               
                return View(_configurationsManager.GetConfigurationDetailInfo());
            }
            catch (Exception ex)
            {
                //todo
            }
            // todo; need to redirect error page or message
            return View();
        }

        #region 'IssueNo'
        [HttpPost]
        public IActionResult CreateIssueNo()
        {
            try
            {
                using (ApplicationDbContext applicationDbContext = new ApplicationDbContext())
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
                        issueNo = applicationDbContext.IssueNos.Include(i => i.IssueTypeFiles).Where(i => i.Id == issueNoId).FirstOrDefault();
                    }
                    
                    issueNo.Name = Request.Form["txtIssueNo"];
                    
                    foreach (IFormFile file in Request.Form.Files)
                    {
                        IssueTypeFile issueTypeFile = new IssueTypeFile();
                        issueTypeFile.Name = file.FileName;
                        issueTypeFile.CreatedBy = User.Identity.Name;
                        issueTypeFile.CreateOn = DateTime.UtcNow;

                        var fileContent = new StringBuilder();
                        using (var reader = new StreamReader(file.OpenReadStream()))
                        {
                            //issueTypeFile.Data = new SqlXml(XmlReader.Create(reader));

                            while (reader.Peek() >= 0)
                                fileContent.AppendLine(reader.ReadLine());
                        }

                        issueTypeFile.Data = fileContent.ToString();
                        issueNo.IssueTypeFiles.Add(issueTypeFile);
                    }

                    if (issueNoId <= 0)
                    {
                        applicationDbContext.IssueNos.Add(issueNo);
                    }

                    applicationDbContext.SaveChanges();

                    return RedirectToAction("Index", "Configuration");
                }
            }
            catch (Exception e)
            {
                return View("failed");
            }
        }
               
        public JsonResult DeleteIssueNo(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
              var issueNo =  applicationContext.IssueNos.Where(i => i.Id ==id).FirstOrDefault();
                if(issueNo != null)
                issueNo.IsDelete = true;
              return Json(_baseManager.CreateOrUpdateRecord(issueNo, "Logical"));
            }
        }
        #endregion

        #region 'Designation'
        public JsonResult CreateDesigination(Designation designation)
        {
            string mode = string.Empty;
            if(designation.Id > 0)
            {
                mode = "Edit";

            }
            else
            {
                var recordCount = _configurationsManager.CheckDuplicateDesination(designation);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(designation, mode));
        }
        public JsonResult DeleteDesignation(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var designation = applicationContext.Designations.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(designation, ""));
            }
        }
        #endregion

        #region 'Info Code Set'
        public JsonResult CreateInfoCodeSet(InformationCodeSet informationCodeSet)
        {
            string mode = string.Empty;

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
                var recordCount = _configurationsManager.CheckDuplicateInfoCodeSet(informationCodeSet);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }

            return Json(_baseManager.CreateOrUpdateRecord(informationCodeSet, mode));
        }

        public JsonResult DeleteInfoCodeSet(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var informationCodeSet = applicationContext.InformationCodeSets.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(informationCodeSet, ""));
            }
        }
        #endregion

        #region 'Info Code'
        public ActionResult LoadDataModuleType()
        {
            using ApplicationDbContext applicationContext = new();
            var dbModuleType = applicationContext.DataModuleTypes.ToList();
            return Json(dbModuleType);
        }
        public JsonResult CreateInfoCode(InformationCode informationCode)
        {         
            string mode = string.Empty;
            if (informationCode.Id > 0)
            {
                informationCode.UpdatedBy = User.Identity.Name;
                informationCode.UpdatedOn = DateTime.UtcNow;
                mode = "Edit";

            }
            else
            {
                //need to update login user email
                informationCode.CreatedBy = User.Identity.Name;
                informationCode.UpdatedBy = User.Identity.Name;
                informationCode.CreatedOn = DateTime.UtcNow;
                informationCode.UpdatedOn = DateTime.UtcNow;
                var recordCount = _configurationsManager.CheckDuplicateInfoCode(informationCode);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }

            return Json(_baseManager.CreateOrUpdateRecord(informationCode, mode));
        }
     
        public JsonResult DeleteInfoCode(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var informationCode = applicationContext.InformationCodes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(informationCode, ""));
            }
        }
        #endregion

        #region 'ICN Format'
        public JsonResult CreateICNFormat(Icnformat icnformat)
        {
            //Fetch all the fields from Database
            List<ICNFormatField> icnFormatFieldsFromDB;
            using (ApplicationDbContext applicationContext = new())
            {
                icnFormatFieldsFromDB = applicationContext.ICNFormatFields.Where(icf => icf.ICNFormatId == icnformat.Id).ToList();

                foreach (ICNFormatField icnFormatField in icnformat.Fields)
                {
                    var icnFormatFieldFromDB = icnFormatFieldsFromDB.Where(ic => ic.ICNFormatMasterFieldId == icnFormatField.ICNFormatMasterFieldId).FirstOrDefault();

                    if (icnFormatFieldFromDB != null && icnFormatFieldFromDB.Id > 0)
                    {
                        icnFormatField.Id = icnFormatFieldFromDB.Id;
                    }
                }
            }

            using (ApplicationDbContext applicationContext = new())
            {
                //if this is an existing format
                if (icnformat.Id > 0)
                {
                    icnformat.UpdatedBy = User.Identity.Name;
                    icnformat.UpdatedOn = DateTime.UtcNow;

                    //if there are existing fields in the database, check if they are there in the paylod. if not, it should be deleted from the db.
                    if (icnformat.Fields != null)
                    {
                        foreach(ICNFormatField field in icnFormatFieldsFromDB)
                        {
                            var formatField = icnformat.Fields.Where(f => f.Id == field.Id).FirstOrDefault();
                            if (formatField == null)
                            {
                                var icnFormatFieldFromDB = icnFormatFieldsFromDB.Where(ic => ic.Id == field.Id).FirstOrDefault();
                                if (icnFormatFieldFromDB != null)
                                {
                                    applicationContext.Entry(icnFormatFieldFromDB).State = EntityState.Deleted;
                                }

                            }
                        }

                        foreach (ICNFormatField icnFormatField in icnformat.Fields)
                        {
                            var icnFormatFieldFromDB = icnFormatFieldsFromDB.Where(ic => ic.ICNFormatMasterFieldId == icnFormatField.ICNFormatMasterFieldId).FirstOrDefault();

                            if (icnFormatFieldFromDB != null && icnFormatFieldFromDB.Id > 0)
                            {
                                icnFormatFieldFromDB.DisplayOrder = icnFormatField.DisplayOrder;
                                applicationContext.Entry(icnFormatFieldFromDB).State = EntityState.Modified;
                            }
                            else
                            {
                                icnFormatField.ICNFormatId = icnformat.Id;
                                applicationContext.Add(icnFormatField);
                            }
                        }
                    }

                    //mark entity as modified.
                    applicationContext.Entry(icnformat).State = EntityState.Modified;
                }
                else
                {
                    //need to update login user email
                    icnformat.CreatedBy = User.Identity.Name;
                    icnformat.UpdatedBy = User.Identity.Name;
                    icnformat.CreatedOn = DateTime.UtcNow;
                    icnformat.UpdatedOn = DateTime.UtcNow;
                    var recordCount = _configurationsManager.CheckDuplicateICTFormat(icnformat);
                    if (recordCount > 0)
                    {
                        return Json("Duplicate");
                    }

                    applicationContext.Icnformats.Add(icnformat);
                }
                applicationContext.SaveChanges();
            }

            return Json(icnformat);
        }
      
        public JsonResult DeleteICNFormat(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var informationCode = applicationContext.Icnformats.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(informationCode, ""));
            }
        }
        #endregion
    }
}
