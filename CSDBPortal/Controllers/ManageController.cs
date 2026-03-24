using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Text.Json;
using System.Xml;
using Project = CSDBPortal.Models.Project;

namespace CSDBPortal.Controllers
{
    public class ManageController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly BaseManager _baseManager;
        private readonly ManageManager _manageManager;
        private readonly BrexValidationEngine _brexValidationEngine;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageController(
            ApplicationDbContext db,
            IWebHostEnvironment appEnvironment,
            BaseManager baseManager,
            ManageManager manageManager,
            BrexValidationEngine brexValidationEngine,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _baseManager = baseManager;
            _manageManager = manageManager;
            _brexValidationEngine = brexValidationEngine;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _manageManager.GetManageDetailInfoAsync());
            }
            catch { }
            return View();
        }

        public async Task<JsonResult> CreateLocationCode(LocationCode locationCode)
        {
            locationCode.CreatedBy = User.Identity.Name;
            locationCode.UpdatedOn = DateTime.UtcNow;
            string mode;
            if (locationCode.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                if (await _manageManager.CheckLocationCodeAsync(locationCode) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(locationCode, mode));
        }

        public async Task<JsonResult> DeleteLocationCode(int id)
        {
            var locationo = await _db.LocationCodes.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(locationo, ""));
        }

        public async Task<JsonResult> CreateLocationCodeSet(LocationCodeSet locationCodeSet)
        {
            locationCodeSet.CreatedBy = User.Identity.Name;
            locationCodeSet.UpdatedOn = DateTime.UtcNow;
            string mode;
            if (locationCodeSet.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                if (await _manageManager.CheckLocationCodeSetAsync(locationCodeSet) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(locationCodeSet, mode));
        }

        public async Task<JsonResult> DeleteLocationCodeSet(int id)
        {
            var locationo = await _db.LocationCodeSets.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(locationo, ""));
        }

        public async Task<JsonResult> CreateRPC(ResponsiblePartnerCode rpc)
        {
            string mode;
            if (rpc.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                if (await _manageManager.CheckRPCAsync(rpc) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(rpc, mode));
        }

        public async Task<JsonResult> DeleteRPC(int id)
        {
            var rpc = await _db.ResponsiblePartnerCodes.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(rpc, ""));
        }

        public async Task<JsonResult> CreateDataTypeModule(DataModuleType dataModuleType)
        {
            dataModuleType.CreatedBy = User.Identity.Name;
            string mode;
            if (dataModuleType.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                if (await _manageManager.CheckDataModuleTypeAsync(dataModuleType) > 0)
                    return Json("Duplicate");
                mode = "Add";
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(dataModuleType, mode));
        }

        public async Task<JsonResult> DeleteDataTypeModule(int id)
        {
            var dataModuleType = await _db.DataModuleTypes.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(dataModuleType, ""));
        }

        public async Task<JsonResult> CreateProject(Project project)
        {
            project.CreatedBy = User.Identity.Name;
            string mode;
            var (recordCount, brexTemplate) = await _manageManager.CheckProjectAsync(project);

            if (project.Id > 0)
            {
                mode = "Edit";
                project.ModifiedBy = User.Identity.Name;
                project.ModifiedDate = DateTime.UtcNow;
            }
            else
            {
                mode = "Add";
                project.CreatedBy = User.Identity.Name;
                project.CreatedDate = DateTime.UtcNow;
                if (recordCount > 0) return Json("Duplicate");
            }

            if (string.IsNullOrEmpty(brexTemplate))
            {
                IssueNo issueNo = await _db.IssueNos.FirstOrDefaultAsync(i => i.Id == project.IssueNoId);
                project.BrexTemplate = issueNo.BrexTemplate;
            }
            else
            {
                project.BrexTemplate = brexTemplate;
            }

            bool projectCreated = await _baseManager.CreateOrUpdateRecordAsync(project, mode);
            if (projectCreated && mode == "Add")
                await _manageManager.CopySNSAsync(project, User.Identity.Name);

            return Json(projectCreated);
        }

        public async Task<JsonResult> DeleteProject(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(i => i.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(project, ""));
        }

        public async Task<JsonResult> CreateSns(StandardNumberingSystem sns)
        {
            string mode;
            if (sns.Id > 0)
            {
                mode = "Edit";
                sns.UpdatedBy = User.Identity.Name;
                sns.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                if (await _manageManager.CheckSnsAsync(sns) > 0)
                    return Json("Duplicate");
                mode = "Add";
                sns.CreatedBy = User.Identity.Name;
                sns.CreatedOn = DateTime.UtcNow;
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(sns, mode));
        }

        public async Task<JsonResult> CreateProjectSns(ProjectStandardNumberingSystem projectSns)
        {
            var projectSnsTemp = await _db.ProjectStandardNumberingSystems
                .FirstOrDefaultAsync(p => p.ProjectId == projectSns.ProjectId && p.Snsid == projectSns.Snsid);
            if (projectSnsTemp != null)
            {
                projectSns.Id = projectSnsTemp.Id;
            }
            else
            {
                projectSns.Snsid = (await _db.ProjectStandardNumberingSystems.MaxAsync(p => p.Snsid)).Value + 1;
            }

            string mode;
            if (projectSns.Id > 0)
            {
                mode = "Edit";
                projectSns.UpdatedBy = User.Identity.Name;
                projectSns.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                if (await _manageManager.CheckProjectSnsAsync(projectSns) > 0)
                    return Json("Duplicate");
                mode = "Add";
                projectSns.CreatedBy = User.Identity.Name;
                projectSns.CreatedOn = DateTime.UtcNow;
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(projectSns, mode));
        }

        public async Task<JsonResult> DeleteSns(int id)
        {
            var sns = await _db.StandardNumberingSystems.FirstOrDefaultAsync(s => s.Id == id);
            return Json(await _baseManager.DeleteRecordAsync(sns, ""));
        }

        public async Task<JsonResult> CreateDMC(DataModuleCode dataModuleCode)
        {
            string mode;
            InformationCode informationCode = await _db.InformationCodes.FirstOrDefaultAsync(i => i.Id == dataModuleCode.InformationCodeId);
            LocationCode locationCode = await _db.LocationCodes.FirstOrDefaultAsync(i => i.Id == dataModuleCode.LocationCodeId);

            string dcValue = string.IsNullOrEmpty(dataModuleCode.DC) ? "-" : "-" + dataModuleCode.DC;
            dataModuleCode.DMC = @"DMC-"
                                + dataModuleCode.ModelIdentification + "-"
                                + dataModuleCode.SDC + "-"
                                + dataModuleCode.StandardNumberingSystem
                                + dcValue
                                + dataModuleCode.DCV + "-"
                                + informationCode.Code
                                + dataModuleCode.ICV + "-"
                                + locationCode.Code;

            if (dataModuleCode.Id > 0)
            {
                mode = "Edit";
                dataModuleCode.UpdatedBy = User.Identity.Name;
                dataModuleCode.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                if (await _manageManager.CheckDMCAsync(dataModuleCode) > 0)
                    return Json("Duplicate");
                mode = "Add";
                dataModuleCode.CreatedBy = User.Identity.Name;
                dataModuleCode.CreatedOn = DateTime.UtcNow;
            }
            return Json(await _baseManager.CreateOrUpdateRecordAsync(dataModuleCode, mode));
        }

        public async Task<JsonResult> DeleteDMC(int dmcId)
        {
            var dataModuleCode = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dataModuleCode != null)
                dataModuleCode.IsDeleted = true;
            return Json(await _db.SaveChangesAsync());
        }

        public async Task<JsonResult> SaveBrexRule(BrexRule brexRule)
        {
            var brexRuleFromDb = await _db.BrexRules.FirstOrDefaultAsync(b => b.Id == brexRule.Id);
            if (brexRuleFromDb == null)
                return Json(new { saved = false, validated = false, count = 0, passed = 0, failed = 0 });

            brexRuleFromDb.Group = brexRule.Group;
            brexRuleFromDb.RuleName = brexRule.RuleName;
            brexRuleFromDb.XmlTag = brexRule.XmlTag;
            brexRuleFromDb.SubXmlTag = brexRule.SubXmlTag;
            brexRuleFromDb.Type = brexRule.Type;
            brexRuleFromDb.Length = brexRule.Length;
            brexRuleFromDb.RangeValue = brexRule.RangeValue;
            brexRuleFromDb.MatchValue = brexRule.MatchValue;
            brexRuleFromDb.AttributeName = brexRule.AttributeName;
            brexRuleFromDb.UpdatedBy = User.Identity.Name;
            brexRuleFromDb.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // Project-wide re-validation: run BREX validation on every DM with generated XML in this project
            var dmIds = await _db.DataModuleCodes
                .Where(d => d.ProjectId == brexRuleFromDb.ProjectId
                         && d.IsBrexXml == false
                         && d.xml != null
                         && d.IsDeleted == false)
                .Select(d => d.Id)
                .ToListAsync();

            if (!dmIds.Any())
                return Json(new { saved = true, validated = false, count = 0, passed = 0, failed = 0 });

            int passedCount = 0, failedCount = 0;
            foreach (var dmId in dmIds)
            {
                var (passed, _) = await _brexValidationEngine.ValidateAsync(dmId, User.Identity.Name);
                if (passed) passedCount++; else failedCount++;
            }

            return Json(new { saved = true, validated = true, count = dmIds.Count, passed = passedCount, failed = failedCount });
        }

        public async Task<JsonResult> GetBrexRules(int projectId)
            => Json(await _manageManager.GetBrexRulesAsync(projectId));

        public async Task<JsonResult> GetProjectNavigationTree(int projectId)
            => Json(await _manageManager.GetProjectNavigationTreeAsync(projectId));

        public async Task<JsonResult> SaveProjectNavigationTree(int projectId, string navigationTreeDataJson)
        {
            List<NavigationTreeData>? navigationTreeData = JsonSerializer.Deserialize<List<NavigationTreeData>>(navigationTreeDataJson);
            await _manageManager.SaveProjectNavigationTreeAsync(projectId, navigationTreeData.ToArray(), User.Identity.Name);
            return Json(await _manageManager.GetProjectNavigationTreeAsync(projectId));
        }

        public async Task<JsonResult> GetProjectNavigation(int projectId)
            => Json(await _manageManager.GetProjectNavigationAsync(projectId));

        public async Task<JsonResult> GetProjectSns(int projectId)
            => Json(await _manageManager.GetProjectSnsAsync(projectId));

        public async Task<JsonResult> GetDataModuleCodes(string dmc, int projectId)
        {
            var codes = await _manageManager.GetDataModuleCodesAsync(projectId);
            return Json(codes.Select(c => new { id = c.Id, dmc = c.DMC, infoName = c.InfoName, techName = c.TechName }));
        }

        public async Task<JsonResult> GetLocationCodes(int projectId)
            => Json(await _manageManager.GetLocationCodesAsync(projectId));

        public async Task<JsonResult> GetInformationCodes(int projectId)
            => Json(await _manageManager.GetInformationCodesAsync(projectId));

        public async Task<JsonResult> GetProject(int projectId)
            => Json(await _manageManager.GetProjectAsync(projectId));

        public async Task<JsonResult> GetSnsCode(int snsId, int projectId)
            => Json(await _manageManager.GetSnsCodeAsync(snsId, projectId));

        public async Task<JsonResult> GetIssueTypeFiles(int projectId)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) return Json(new List<IssueTypeFile>());
            var issues = await _db.IssueTypeFiles.Where(p => p.IssueNoId == project.IssueNoId).ToListAsync();
            return Json(issues);
        }

        public FileResult DownloadDMCTemplate()
        {
            StringBuilder csvHeader = new StringBuilder();
            csvHeader.Append("Project Name,Tech Name,Info Name,Schema File,SNS,DC,DCV,Information Code,ICV,Location Code");
            return File(Encoding.UTF8.GetBytes(csvHeader.ToString()), "text/plain", "DMCTemplate.csv");
        }

        [HttpPost]
        public async Task<IActionResult> UploadDMC()
        {
            List<string> successfulIds = new List<string>();
            List<string> failedIds = new List<string>();

            foreach (IFormFile file in Request.Form.Files)
            {
                using (TextFieldParser parser = new TextFieldParser(file.OpenReadStream()))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    bool isHeaderCompleted = false;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (isHeaderCompleted)
                        {
                            try
                            {
                                Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Name == fields[0]);
                                InformationCode informationCode = await _db.InformationCodes.FirstOrDefaultAsync(i => i.Code == fields[7] && i.InformationCodeSetId == project.InformationCodeId);
                                LocationCode locationCode = await _db.LocationCodes.FirstOrDefaultAsync(l => l.Code == fields[9] && l.LocationCodeSetId == project.LocationCodeId);
                                IssueTypeFile issueTypeFile = await _db.IssueTypeFiles.FirstOrDefaultAsync(f => f.Name == fields[3]);

                                string dcValue = string.IsNullOrEmpty(fields[5]) ? "-" : "-" + fields[5];
                                string dmCode = @"DMC-" + project.ModelIdentification + "-" + project.SDC + "-"
                                                + fields[4] + dcValue + fields[6] + "-"
                                                + informationCode.Code + fields[8] + "-" + locationCode.Code;

                                _db.DataModuleCodes.Add(new DataModuleCode()
                                {
                                    CreatedBy = User.Identity.Name, CreatedOn = DateTime.UtcNow,
                                    DC = fields[5], DCV = fields[6], ICV = fields[8],
                                    InfoName = fields[2], InformationCodeId = informationCode.Id,
                                    IssueFileId = issueTypeFile.Id, LocationCodeId = locationCode.Id,
                                    ModelIdentification = project.ModelIdentification, ProjectId = project.Id,
                                    SDC = project.SDC, StandardNumberingSystem = fields[4],
                                    TechName = fields[1], DMC = dmCode
                                });
                                successfulIds.Add(String.Join(",", fields));
                            }
                            catch
                            {
                                failedIds.Add(String.Join(",", fields));
                            }
                        }
                        isHeaderCompleted = true;
                    }
                }
            }
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Manage");
        }

        // Opens the XML inline in a new browser tab (default XML viewer / Notepad fallback).
        public async Task<ContentResult> ViewDMCXml(int dmcId)
        {
            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            return Content(dmc.xml, "text/xml", Encoding.UTF8);
        }

        // Downloads the XML file so the OS opens it in the default XML editor (or Notepad).
        public async Task<FileResult> DownloadDMCXml(int dmcId)
        {
            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            return File(Encoding.UTF8.GetBytes(dmc.xml), "text/xml", dmc.DMC + ".xml");
        }

        [HttpPost]
        public async Task<JsonResult> CheckoutDMC(int dmcId)
        {
            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null)
                return Json(new { status = false, message = "DMC not found." });
            if (dmc.CheckoutStatus == "CheckedOut")
                return Json(new { status = false, message = $"Already checked out by {dmc.CheckedOutBy}." });

            dmc.CheckoutStatus = "CheckedOut";
            dmc.CheckedOutBy = User.Identity.Name;
            dmc.CheckedOutOn = DateTime.UtcNow;
            dmc.OriginalXml = dmc.xml;
            await _db.SaveChangesAsync();

            return Json(new { status = true });
        }

        [HttpPost]
        public async Task<JsonResult> CheckinDMC(int dmcId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { status = false, message = "No file uploaded." });

            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null)
                return Json(new { status = false, message = "DMC not found." });
            if (dmc.CheckoutStatus != "CheckedOut")
                return Json(new { status = false, message = "This DMC is not currently checked out." });

            string uploadedXml;
            using (var reader = new System.IO.StreamReader(file.OpenReadStream(), Encoding.UTF8))
                uploadedXml = await reader.ReadToEndAsync();

            // Validate XML is well-formed
            XmlDocument uploadedDoc = new XmlDocument();
            try { uploadedDoc.LoadXml(uploadedXml); }
            catch (Exception ex)
                { return Json(new { status = false, message = $"Uploaded file is not valid XML: {ex.Message}" }); }

            // Constraint 1 — identAndStatusSection must be unchanged
            if (!string.IsNullOrEmpty(dmc.OriginalXml))
            {
                try
                {
                    XmlDocument originalDoc = new XmlDocument();
                    originalDoc.LoadXml(dmc.OriginalXml);

                    string origSection = NormalizeXml(
                        originalDoc.SelectSingleNode("//identAndStatusSection")?.OuterXml ?? string.Empty);
                    string newSection = NormalizeXml(
                        uploadedDoc.SelectSingleNode("//identAndStatusSection")?.OuterXml ?? string.Empty);

                    if (!string.Equals(origSection, newSection, StringComparison.Ordinal))
                        return Json(new { status = false, message = "Check-in rejected: the identAndStatusSection has been modified. Restore it to its original state and try again." });
                }
                catch { /* OriginalXml unparseable — skip comparison */ }
            }

            // Constraint 2 — BREX validation must pass
            var (brexPassed, brexMessage) = await _brexValidationEngine.ValidateXmlStringAsync(dmc.ProjectId, uploadedXml);
            if (!brexPassed)
                return Json(new { status = false, message = $"Check-in rejected: BREX validation failed. {brexMessage}" });

            // All constraints passed — complete check-in
            dmc.xml = uploadedXml;
            dmc.CheckoutStatus = null;
            dmc.CheckedOutBy = null;
            dmc.CheckedOutOn = null;
            dmc.OriginalXml = null;
            dmc.UpdatedBy = User.Identity.Name;
            dmc.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _brexValidationEngine.RecordResultAsync(dmcId, true, brexMessage, User.Identity.Name);

            return Json(new { status = true, message = "Check-in successful. BREX validation passed." });
        }

        private static string NormalizeXml(string xml) =>
            System.Text.RegularExpressions.Regex.Replace(xml, @"\s+", " ").Trim();

        public async Task<JsonResult> GenerateBrexXml(int projectId)
        {
            try
            {
                DataModuleCode dmcCode = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.IsBrexXml == true && d.ProjectId == projectId);
                if (dmcCode != null)
                    return new JsonResult(new { Status = false, Message = "Brex XML is already available for this Project!" });

                Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                ResponsiblePartnerCode rpc = await _db.ResponsiblePartnerCodes.FirstOrDefaultAsync(r => r.Id == project.RPCId);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(project.BrexTemplate);
                XmlNamespaceManager xMan = new XmlNamespaceManager(xmlDoc.NameTable);

                StringBuilder dmcString = new StringBuilder("DMC-");
                dmcString.Append(project.ModelIdentification).Append("-").Append(project.SDC).Append("-");

                string informationCodeString = string.Empty, locationCodeString = string.Empty;
                string snsString = string.Empty, infoNameString = string.Empty, techNameString = string.Empty;

                XmlNode node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/dmCode", xMan);
                if (node != null)
                {
                    dmcString.Append(node.Attributes["systemCode"].Value).Append("-")
                             .Append(node.Attributes["disassyCode"].Value).Append("-")
                             .Append(node.Attributes["disassyCodeVariant"].Value).Append("-")
                             .Append(node.Attributes["infoCode"].Value).Append("-")
                             .Append(node.Attributes["infoCodeVariant"].Value).Append("-")
                             .Append(node.Attributes["itemLocationCode"].Value).Append("-");
                    informationCodeString = node.Attributes["infoCode"].Value;
                    locationCodeString = node.Attributes["itemLocationCode"].Value;
                    snsString = node.Attributes["systemCode"].Value;
                    node.Attributes["modelIdentCode"].Value = project.ModelIdentification;
                    node.Attributes["subSubSystemCode"].Value = "0";
                    node.Attributes["subSystemCode"].Value = "0";
                    node.Attributes["systemDiffCode"].Value = project.SDC;
                }

                SetXmlNodeText(xmlDoc, xMan, "/dmodule[@*]/Description/title", out techNameString);
                SetXmlNodeText(xmlDoc, xMan, "/dmodule[@*]/Description/subject", out infoNameString);
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/creator", rpc.Rpccage);
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/publisher", rpc.Rpccage);
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/contributor", rpc.Rpccage);
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/date", DateTime.Now.Date.ToString("yyyy-MM-dd"));
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/type", "text");
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/format", "text / xml");
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/identifier", dmcString.ToString());
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/language", "en-US");
                SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/rights", "01_cc51");

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/language", xMan);
                if (node != null) { node.Attributes["countryIsoCode"].Value = "US"; node.Attributes["languageIsoCode"].Value = "en"; }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/issueInfo", xMan);
                if (node != null) { node.Attributes["inWork"].Value = "00"; node.Attributes["issueNumber"].Value = "001"; }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/issueDate", xMan);
                if (node != null)
                {
                    node.Attributes["day"].Value = DateTime.Now.Day.ToString("D2");
                    node.Attributes["month"].Value = DateTime.Now.Month.ToString("D2");
                    node.Attributes["year"].Value = DateTime.Now.Year.ToString("D4");
                }

                StringWriter stringWriter = new StringWriter();
                xmlDoc.WriteTo(new XmlTextWriter(stringWriter));

                InformationCode informationCode = await _db.InformationCodes.FirstOrDefaultAsync(i => i.Code == informationCodeString);
                LocationCode locationCode = await _db.LocationCodes.FirstOrDefaultAsync(i => i.Code == locationCodeString);

                _db.DataModuleCodes.Add(new DataModuleCode()
                {
                    IsDeleted = false, InformationCodeId = informationCode.Id, LocationCodeId = locationCode.Id,
                    CreatedBy = User.Identity.Name, CreatedOn = DateTime.UtcNow, DC = string.Empty,
                    DCV = string.Empty, DMC = dmcString.ToString(), ICV = string.Empty,
                    InfoName = infoNameString, IssueFileId = 1, ModelIdentification = project.ModelIdentification,
                    ProjectId = project.Id, SDC = project.SDC, StandardNumberingSystem = snsString,
                    TechName = techNameString, IsBrexXml = true, xml = stringWriter.ToString()
                });
                await _db.SaveChangesAsync();
                return new JsonResult(new { Status = true, Message = "Brex XML Created Successfully!" });
            }
            catch
            {
                return new JsonResult(new { Status = false, Message = "Error while generating Brex XML!" });
            }
        }

        public async Task<JsonResult> GenerateDMCXml(int dmcId)
        {
            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == dmc.ProjectId);
            ResponsiblePartnerCode rpc = await _db.ResponsiblePartnerCodes.FirstOrDefaultAsync(r => r.Id == project.RPCId);
            InformationCode informationCode = await _db.InformationCodes.FirstOrDefaultAsync(i => i.Id == dmc.InformationCodeId);
            LocationCode locationCode = await _db.LocationCodes.FirstOrDefaultAsync(l => l.Id == dmc.LocationCodeId);
            IssueTypeFile issueTypeFile = await _db.IssueTypeFiles.FirstOrDefaultAsync(i => i.Id == dmc.IssueFileId);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(project.BrexTemplate);
            XmlNamespaceManager xMan = new XmlNamespaceManager(xmlDoc.NameTable);

            XmlNode node = xmlDoc.SelectSingleNode("/dmodule", xMan);
            if (node != null) node.Attributes["xsi:noNamespaceSchemaLocation"].Value = "http://www.s1000d.org/S1000D_4-2/xml_schema_flat/" + issueTypeFile.Name;

            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/creator", rpc.Rpccage);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/title", dmc.TechName);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/subject", dmc.InfoName);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/publisher", rpc.Rpccage);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/contributor", rpc.Rpccage);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/date", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/type", "text");
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/format", "text / xml");
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/identifier", dmc.DMC);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/language", "en-US");
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/Description/rights", "01_cc51");

            node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/language", xMan);
            if (node != null) { node.Attributes["countryIsoCode"].Value = "US"; node.Attributes["languageIsoCode"].Value = "en"; }

            node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/issueInfo", xMan);
            if (node != null) { node.Attributes["inWork"].Value = "00"; node.Attributes["issueNumber"].Value = "001"; }

            node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/issueDate", xMan);
            if (node != null)
            {
                node.Attributes["day"].Value = DateTime.Now.Day.ToString("D2");
                node.Attributes["month"].Value = DateTime.Now.Month.ToString("D2");
                node.Attributes["year"].Value = DateTime.Now.Year.ToString("D4");
            }

            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/dmTitle/techName", dmc.TechName);
            SetXmlNodeInnerText(xmlDoc, xMan, "/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/dmTitle/infoName", dmc.InfoName);

            node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmStatus/brexDmRef/dmRef/dmRefIdent/dmCode", xMan);
            if (node != null)
            {
                node.Attributes["assyCode"].Value = dmc.DC;
                node.Attributes["disassyCode"].Value = dmc.DC;
                node.Attributes["disassyCodeVariant"].Value = dmc.DCV;
                node.Attributes["infoCode"].Value = informationCode.Code;
                node.Attributes["infoCodeVariant"].Value = dmc.ICV;
                node.Attributes["itemLocationCode"].Value = locationCode.Code;
                node.Attributes["modelIdentCode"].Value = dmc.ModelIdentification;
                node.Attributes["subSubSystemCode"].Value = "0";
                node.Attributes["subSystemCode"].Value = "0";
                node.Attributes["systemCode"].Value = dmc.StandardNumberingSystem;
                node.Attributes["systemDiffCode"].Value = dmc.SDC;
            }

            StringWriter stringWriter = new StringWriter();
            xmlDoc.WriteTo(new XmlTextWriter(stringWriter));
            dmc.xml = stringWriter.ToString();
            await _db.SaveChangesAsync();

            // Auto-validate against BREX rules after generating XML
            var (passed, message) = await _brexValidationEngine.ValidateAsync(dmc.Id, User.Identity.Name);
            return new JsonResult(new { status = passed, message });
        }

        [HttpPost]
        public async Task<JsonResult> ValidateDMC(int dmcId)
        {
            var (passed, message) = await _brexValidationEngine.ValidateAsync(dmcId, User.Identity.Name);
            return Json(new { status = passed, message });
        }

        // ── Allocation endpoints ───────────────────────────────────────────────

        public JsonResult GetAllocationRoles()
        {
            var roles = _roleManager.Roles
                .Select(r => new { r.Id, r.Name })
                .OrderBy(r => r.Name)
                .ToList();
            return Json(roles);
        }

        public async Task<JsonResult> GetAllocationUsers(string roleId = null)
        {
            IList<IdentityUser> users;
            if (!string.IsNullOrEmpty(roleId))
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                users = role != null
                    ? await _userManager.GetUsersInRoleAsync(role.Name)
                    : new List<IdentityUser>();
            }
            else
            {
                users = _userManager.Users.OrderBy(u => u.UserName).ToList();
            }
            return Json(users.Select(u => new { u.Id, u.UserName, u.Email }));
        }

        public async Task<JsonResult> GetDMCsForAllocation(int projectId)
        {
            var dmcs = await _db.DataModuleCodes
                .Where(d => d.ProjectId == projectId && !d.IsDeleted && !d.IsBrexXml)
                .Select(d => new { d.Id, d.DMC, d.AssignedTo })
                .ToListAsync();
            return Json(dmcs);
        }

        public async Task<JsonResult> GetICNsForAllocation(int projectId)
        {
            var icns = await _db.IcnNumbers
                .Where(i => i.ProjectId == projectId)
                .Select(i => new { i.Id, i.Number, i.AssignedTo })
                .ToListAsync();
            return Json(icns);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDMCAssignment(int dmcId, string userId)
        {
            var dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null) return Json(new { status = false });
            dmc.AssignedTo = string.IsNullOrEmpty(userId) ? null : userId;
            dmc.UpdatedBy = User.Identity.Name;
            dmc.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Json(new { status = true });
        }

        [HttpPost]
        public async Task<JsonResult> SaveICNAssignment(int icnId, string userId)
        {
            var icn = await _db.IcnNumbers.FirstOrDefaultAsync(i => i.Id == icnId);
            if (icn == null) return Json(new { status = false });
            icn.AssignedTo = string.IsNullOrEmpty(userId) ? null : userId;
            icn.UpdatedBy = User.Identity.Name;
            icn.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Json(new { status = true });
        }

        private void SetXmlNodeInnerText(XmlDocument doc, XmlNamespaceManager xMan, string xpath, string value)
        {
            XmlNode node = doc.SelectSingleNode(xpath, xMan);
            if (node != null) node.InnerText = value;
        }

        private void SetXmlNodeText(XmlDocument doc, XmlNamespaceManager xMan, string xpath, out string value)
        {
            XmlNode node = doc.SelectSingleNode(xpath, xMan);
            value = node?.InnerText ?? string.Empty;
        }

        // ── Stylesheet endpoints ───────────────────────────────────────────────

        [HttpPost]
        public async Task<JsonResult> UploadStylesheet(string name, string? remarks, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { status = false, message = "No file selected." });

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".xsl" && ext != ".xslt")
                return Json(new { status = false, message = "Only .xsl or .xslt files are accepted." });

            string content;
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                content = await reader.ReadToEndAsync();

            // Basic XML well-formedness check
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(content);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Invalid XML: {ex.Message}" });
            }

            var stylesheet = new Models.Stylesheet
            {
                Name       = name?.Trim() ?? Path.GetFileNameWithoutExtension(file.FileName),
                FileName   = file.FileName,
                Content    = content,
                Remarks    = remarks?.Trim(),
                UploadedBy = User.Identity!.Name ?? string.Empty,
                UploadedOn = DateTime.UtcNow
            };

            _db.Stylesheets.Add(stylesheet);
            await _db.SaveChangesAsync();

            return Json(new
            {
                status = true,
                id         = stylesheet.Id,
                name       = stylesheet.Name,
                fileName   = stylesheet.FileName,
                remarks    = stylesheet.Remarks,
                uploadedBy = stylesheet.UploadedBy,
                uploadedOn = stylesheet.UploadedOn.ToString("yyyy-MM-dd HH:mm")
            });
        }

        public async Task<JsonResult> GetStylesheets()
        {
            var list = await _db.Stylesheets
                .OrderByDescending(s => s.UploadedOn)
                .Select(s => new
                {
                    s.Id, s.Name, s.FileName, s.Remarks,
                    s.UploadedBy,
                    UploadedOn = s.UploadedOn.ToString("yyyy-MM-dd HH:mm"),
                    UpdatedBy  = s.UpdatedBy,
                    UpdatedOn  = s.UpdatedOn != null ? s.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm") : (string?)null
                })
                .ToListAsync();
            return Json(list);
        }

        public async Task<JsonResult> GetStylesheetContent(int id)
        {
            var ss = await _db.Stylesheets.FindAsync(id);
            if (ss == null) return Json(new { status = false });
            return Json(new { status = true, id = ss.Id, name = ss.Name, content = ss.Content, remarks = ss.Remarks });
        }

        [HttpPost]
        public async Task<JsonResult> SaveStylesheetContent(int id, string content, string? remarks)
        {
            var ss = await _db.Stylesheets.FindAsync(id);
            if (ss == null) return Json(new { status = false, message = "Not found." });

            // Validate XML before saving
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(content);
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = $"Invalid XML: {ex.Message}" });
            }

            ss.Content   = content;
            ss.Remarks   = remarks?.Trim();
            ss.UpdatedBy = User.Identity!.Name;
            ss.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Json(new { status = true });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteStylesheet(int id)
        {
            var ss = await _db.Stylesheets.FindAsync(id);
            if (ss == null) return Json(new { status = false });
            _db.Stylesheets.Remove(ss);
            await _db.SaveChangesAsync();
            return Json(new { status = true });
        }

        public async Task<IActionResult> DownloadStylesheet(int id)
        {
            var ss = await _db.Stylesheets.FindAsync(id);
            if (ss == null) return NotFound();
            var bytes = Encoding.UTF8.GetBytes(ss.Content ?? string.Empty);
            return File(bytes, "application/xml", ss.FileName);
        }

        // ── Image Asset endpoints ──────────────────────────────────────────────

        private static readonly HashSet<string> _allowedImageMimes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/gif", "image/svg+xml",
            "image/webp", "image/bmp", "image/tiff"
        };

        [HttpPost]
        public async Task<JsonResult> UploadImageAsset(string name, string? remarks, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { status = false, message = "No file selected." });

            var mimeType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
            if (!_allowedImageMimes.Contains(mimeType))
                return Json(new { status = false, message = "Unsupported file type. Accepted: JPEG, PNG, GIF, SVG, WebP, BMP, TIFF." });

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            var asset = new Models.ImageAsset
            {
                Name       = name?.Trim() ?? Path.GetFileNameWithoutExtension(file.FileName),
                FileName   = file.FileName,
                MimeType   = mimeType,
                Data       = Convert.ToBase64String(bytes),
                Remarks    = remarks?.Trim(),
                UploadedBy = User.Identity!.Name ?? string.Empty,
                UploadedOn = DateTime.UtcNow
            };

            _db.ImageAssets.Add(asset);
            await _db.SaveChangesAsync();

            return Json(new
            {
                status     = true,
                id         = asset.Id,
                name       = asset.Name,
                fileName   = asset.FileName,
                mimeType   = asset.MimeType,
                remarks    = asset.Remarks,
                uploadedBy = asset.UploadedBy,
                uploadedOn = asset.UploadedOn.ToString("yyyy-MM-dd HH:mm"),
                dataUrl    = $"data:{asset.MimeType};base64,{asset.Data}"
            });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateImageRemarks(int id, string? remarks)
        {
            var asset = await _db.ImageAssets.FindAsync(id);
            if (asset == null) return Json(new { status = false });
            asset.Remarks   = remarks?.Trim();
            asset.UpdatedBy = User.Identity!.Name;
            asset.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Json(new { status = true });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteImageAsset(int id)
        {
            var asset = await _db.ImageAssets.FindAsync(id);
            if (asset == null) return Json(new { status = false });
            _db.ImageAssets.Remove(asset);
            await _db.SaveChangesAsync();
            return Json(new { status = true });
        }

        public async Task<IActionResult> DownloadImageAsset(int id)
        {
            var asset = await _db.ImageAssets.FindAsync(id);
            if (asset == null) return NotFound();
            var bytes = Convert.FromBase64String(asset.Data ?? string.Empty);
            return File(bytes, asset.MimeType, asset.FileName);
        }

        public async Task<IActionResult> ViewImageAsset(int id)
        {
            var asset = await _db.ImageAssets.FindAsync(id);
            if (asset == null) return NotFound();
            var bytes = Convert.FromBase64String(asset.Data ?? string.Empty);
            return File(bytes, asset.MimeType);
        }

        // ── ID Manager ──────────────────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetIdManagerData(int projectId)
        {
            try
            {
                var dmcs = await _db.DataModuleCodes
                    .Where(d => d.ProjectId == projectId && !d.IsDeleted && d.xml != null)
                    .Select(d => new { d.Id, d.DMC, d.xml })
                    .AsNoTracking()
                    .ToListAsync();

                string[] refAttrs = { "xrefid", "internalRefId", "refid", "targetId", "applicRefId", "condRefId" };

                // Single pass per DMC: collect id/xml:id declarations and reference attribute values together
                var allEntries = new List<(string IdVal, int DmcId, string DmcCode)>();
                var referencedIds = new HashSet<string>(StringComparer.Ordinal);

                foreach (var dmc in dmcs)
                {
                    try
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(dmc.xml!);

                        foreach (XmlNode node in doc.SelectNodes("//*")!)
                        {
                            if (node.Attributes == null) continue;

                            // Collect declared IDs (plain id or xml:id)
                            var idVal = node.Attributes["id"]?.Value
                                     ?? node.Attributes["xml:id"]?.Value;
                            if (!string.IsNullOrWhiteSpace(idVal))
                                allEntries.Add((idVal, dmc.Id, dmc.DMC ?? ""));

                            // Collect referenced IDs from reference attributes
                            foreach (var attr in refAttrs)
                            {
                                var refVal = node.Attributes[attr]?.Value;
                                if (!string.IsNullOrEmpty(refVal))
                                    referencedIds.Add(refVal);
                            }
                        }
                    }
                    catch { }
                }

                // Group by ID value and classify
                var rows = allEntries
                    .GroupBy(e => e.IdVal, StringComparer.Ordinal)
                    .Select(g =>
                    {
                        bool isDup = g.Count() > 1;
                        bool isRef = referencedIds.Contains(g.Key);
                        return new
                        {
                            idValue     = g.Key,
                            status      = isDup ? "Duplicate" : isRef ? "Referenced" : "Unused",
                            occurrences = g.Select(e => new { e.DmcId, e.DmcCode }).ToList()
                        };
                    })
                    .OrderBy(r => r.idValue)
                    .ToList();

                return Json(rows);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.Message });
            }
        }
    }
}
