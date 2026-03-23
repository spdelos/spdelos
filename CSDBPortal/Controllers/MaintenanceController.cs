using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Text.Json;
using System.Xml;
using Project = CSDBPortal.Models.Project;

namespace CSDBPortal.Controllers
{
    public class MaintenanceController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly BaseManager _baseManager;
        private readonly MaintenanceManager _maintenanceManager;
        private readonly BrexValidationEngine _brexValidationEngine;

        public MaintenanceController(
            ApplicationDbContext db,
            IWebHostEnvironment appEnvironment,
            BaseManager baseManager,
            MaintenanceManager maintenanceManager,
            BrexValidationEngine brexValidationEngine)
        {
            _db = db;
            _appEnvironment = appEnvironment;
            _baseManager = baseManager;
            _maintenanceManager = maintenanceManager;
            _brexValidationEngine = brexValidationEngine;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _maintenanceManager.GetMaintenanceDetailInfoAsync());
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
                if (await _maintenanceManager.CheckLocationCodeAsync(locationCode) > 0)
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
                if (await _maintenanceManager.CheckLocationCodeSetAsync(locationCodeSet) > 0)
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
                if (await _maintenanceManager.CheckRPCAsync(rpc) > 0)
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
                if (await _maintenanceManager.CheckDataModuleTypeAsync(dataModuleType) > 0)
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
            var (recordCount, brexTemplate) = await _maintenanceManager.CheckProjectAsync(project);

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
                await _maintenanceManager.CopySNSAsync(project, User.Identity.Name);

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
                if (await _maintenanceManager.CheckSnsAsync(sns) > 0)
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
                if (await _maintenanceManager.CheckProjectSnsAsync(projectSns) > 0)
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
                if (await _maintenanceManager.CheckDMCAsync(dataModuleCode) > 0)
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
            => Json(await _maintenanceManager.GetBrexRulesAsync(projectId));

        public async Task<JsonResult> GetProjectNavigationTree(int projectId)
            => Json(await _maintenanceManager.GetProjectNavigationTreeAsync(projectId));

        public async Task<JsonResult> SaveProjectNavigationTree(int projectId, string navigationTreeDataJson)
        {
            List<NavigationTreeData>? navigationTreeData = JsonSerializer.Deserialize<List<NavigationTreeData>>(navigationTreeDataJson);
            await _maintenanceManager.SaveProjectNavigationTreeAsync(projectId, navigationTreeData.ToArray(), User.Identity.Name);
            return Json(await _maintenanceManager.GetProjectNavigationTreeAsync(projectId));
        }

        public async Task<JsonResult> GetProjectNavigation(int projectId)
            => Json(await _maintenanceManager.GetProjectNavigationAsync(projectId));

        public async Task<JsonResult> GetProjectSns(int projectId)
            => Json(await _maintenanceManager.GetProjectSnsAsync(projectId));

        public async Task<JsonResult> GetDataModuleCodes(string dmc, int projectId)
        {
            var codes = await _maintenanceManager.GetDataModuleCodesAsync(projectId);
            return Json(codes.Select(c => new { id = c.Id, dmc = c.DMC, infoName = c.InfoName, techName = c.TechName }));
        }

        public async Task<JsonResult> GetLocationCodes(int projectId)
            => Json(await _maintenanceManager.GetLocationCodesAsync(projectId));

        public async Task<JsonResult> GetInformationCodes(int projectId)
            => Json(await _maintenanceManager.GetInformationCodesAsync(projectId));

        public async Task<JsonResult> GetProject(int projectId)
            => Json(await _maintenanceManager.GetProjectAsync(projectId));

        public async Task<JsonResult> GetSnsCode(int snsId, int projectId)
            => Json(await _maintenanceManager.GetSnsCodeAsync(snsId, projectId));

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

            return RedirectToAction("Index", "Maintenance");
        }

        public async Task<FileResult> ViewDMCXml(int dmcId)
        {
            DataModuleCode dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            return File(Encoding.UTF8.GetBytes(dmc.xml), "text/xml", dmc.DMC + ".xml");
        }

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
    }
}
