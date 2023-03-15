using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xml.XMLGen;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Project = CSDBPortal.Models.Project;

namespace CSDBPortal.Controllers
{
    public class MaintenanceController : BaseController
    {
        private readonly IWebHostEnvironment appEnvironment;

        BaseManager _baseManager = new();
        MaintenanceManager maintenancesManager = new();

        public MaintenanceController(IWebHostEnvironment appEnvironmentvalue)
        {
            this.appEnvironment = appEnvironmentvalue;
        }

        //[Authorize]
        public IActionResult Index()
        {
            try
            {
                MaintenanceManager _maintenanceManager = new();
                return View(_maintenanceManager.GetMaintenanceDetailInfo());
            }
            catch (Exception ex)
            {
                //todo
            }
            // todo; need to redirect error page or message
            return View();
        }

        public JsonResult CreateLocationCode(LocationCode locationCode)
        {
            // need to assign login user email here
            locationCode.CreatedBy = User.Identity.Name;
            locationCode.UpdatedOn = DateTime.UtcNow;
            string mode = string.Empty;
            if (locationCode.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckLocationCode(locationCode);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(locationCode, mode));
        }

        public JsonResult DeleteLocationCode(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.LocationCodes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }

        public JsonResult CreateLocationCodeSet(LocationCodeSet locationCodeSet)
        {
            // need to assign login user email here
            locationCodeSet.CreatedBy = User.Identity.Name;
            locationCodeSet.UpdatedOn = DateTime.UtcNow;
            string mode = string.Empty;
            if (locationCodeSet.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckLocationCodeSet(locationCodeSet);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(locationCodeSet, mode));
        }

        public JsonResult DeleteLocationCodeSet(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.LocationCodeSets.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }

        public JsonResult CreateRPC(ResponsiblePartnerCode rpc)
        {
            // need to assign login user email here

            string mode = string.Empty;
            if (rpc.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckRPC(rpc);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(rpc, mode));
        }

        public JsonResult DeleteRPC(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var rpc = applicationContext.ResponsiblePartnerCodes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(rpc, ""));
            }
        }



        public JsonResult CreateDataTypeModule(DataModuleType dataModuleType)
        {
            // need to assign login user email here
            dataModuleType.CreatedBy = User.Identity.Name;
            string mode = string.Empty;
            if (dataModuleType.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckDataModuleType(dataModuleType);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(dataModuleType, mode));
        }

        public JsonResult DeleteDataTypeModule(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var dataModuleType = applicationContext.DataModuleTypes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(dataModuleType, ""));
            }
        }

        public JsonResult CreateProject(Project project)
        {
            project.CreatedBy = User.Identity.Name;

            string mode = string.Empty;
            string brexTemplate = string.Empty;

            var recordCount = maintenancesManager.CheckProject(project, out brexTemplate);

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

                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
            }

            if (string.IsNullOrEmpty(brexTemplate))
            {
                using (ApplicationDbContext applicationContext = new())
                {
                    IssueNo issueNo = applicationContext.IssueNos.Where(i => i.Id == project.IssueNoId).FirstOrDefault();
                    project.BrexTemplate = issueNo.BrexTemplate;
                }
            }
            else
            {
                project.BrexTemplate = brexTemplate;
            }

            bool projectCreated = _baseManager.CreateOrUpdateRecord(project, mode);
            if (projectCreated == true && mode == "Add")
            {
                maintenancesManager.CopySNS(project, User.Identity.Name);
            }

            return Json(projectCreated);
        }

        public JsonResult DeleteProject(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var project = applicationContext.Projects.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(project, ""));
            }
        }

        public JsonResult CreateSns(StandardNumberingSystem sns)
        {
            string mode = string.Empty;
            if (sns.Id > 0)
            {
                mode = "Edit";
                sns.UpdatedBy = User.Identity.Name;
                sns.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                var recordCount = maintenancesManager.CheckSns(sns);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
                sns.CreatedBy = User.Identity.Name;
                sns.CreatedOn = DateTime.UtcNow;
            }
            return Json(_baseManager.CreateOrUpdateRecord(sns, mode));
        }

        public JsonResult CreateProjectSns(ProjectStandardNumberingSystem projectSns)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                ProjectStandardNumberingSystem projectSnsTemp = applicationContext.ProjectStandardNumberingSystems.Where(p => p.ProjectId == projectSns.ProjectId && p.Snsid == projectSns.Snsid).FirstOrDefault();
                if (projectSnsTemp != null)
                {
                    projectSns.Id = projectSnsTemp.Id;
                }
                else
                {
                    projectSns.Snsid = applicationContext.ProjectStandardNumberingSystems.Max(p => p.Snsid).Value + 1;
                }
            }

            string mode = string.Empty;
            if (projectSns.Id > 0)
            {
                mode = "Edit";
                projectSns.UpdatedBy = User.Identity.Name;
                projectSns.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                var recordCount = maintenancesManager.CheckProjectSns(projectSns);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
                projectSns.CreatedBy = User.Identity.Name;
                projectSns.CreatedOn = DateTime.UtcNow;
            }
            return Json(_baseManager.CreateOrUpdateRecord(projectSns, mode));
        }

        public JsonResult DeleteSns(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.StandardNumberingSystems.Where(s => s.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }

        public JsonResult CreateDMC(DataModuleCode dataModuleCode)
        {
            string mode = string.Empty;
            if (dataModuleCode.Id > 0)
            {
                mode = "Edit";
                dataModuleCode.UpdatedBy = User.Identity.Name;
                dataModuleCode.UpdatedOn = DateTime.UtcNow;
            }
            else
            {
                var recordCount = maintenancesManager.CheckDMC(dataModuleCode);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
                dataModuleCode.CreatedBy = User.Identity.Name;
                dataModuleCode.CreatedOn = DateTime.UtcNow;
            }

            using (ApplicationDbContext applicationContext = new())
            {
                InformationCode informationCode = applicationContext.InformationCodes.Where(i => i.Id == dataModuleCode.InformationCodeId).FirstOrDefault();
                LocationCode locationCode = applicationContext.LocationCodes.Where(i => i.Id == dataModuleCode.LocationCodeId).FirstOrDefault();

                string dcValue = string.IsNullOrEmpty(dataModuleCode.DC) ? "-" : "-" + dataModuleCode.DC;

                dataModuleCode.DMC = @"DMC -"
                                        + dataModuleCode.ModelIdentification + "-"
                                        + dataModuleCode.SDC + "-"
                                        + dataModuleCode.StandardNumberingSystem
                                        + dcValue
                                        + dataModuleCode.DCV + "-"
                                        + informationCode.Code
                                        + dataModuleCode.ICV + "-"
                                        + locationCode.Code;
            }

            return Json(_baseManager.CreateOrUpdateRecord(dataModuleCode, mode));
        }

        public JsonResult DeleteDMC(int dmcId)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var dataModuleCode = applicationContext.DataModuleCodes.Where(d => d.Id == dmcId).FirstOrDefault();
                if (dataModuleCode != null)
                {
                    dataModuleCode.IsDeleted = true;
                }

                return Json(applicationContext.SaveChanges());
            }
        }

        public JsonResult GetProjectSns(int projectId)
        {
            return Json(maintenancesManager.GetProjectSns(projectId));
        }

        public JsonResult GetLocationCodes(int projectId)
        {
            return Json(maintenancesManager.GetLocationCodes(projectId));
        }

        public JsonResult GetInformationCodes(int projectId)
        {
            return Json(maintenancesManager.GetInformationCodes(projectId));
        }

        public JsonResult GetProject(int projectId)
        {
            return Json(maintenancesManager.GetProject(projectId));
        }

        public JsonResult GetSnsCode(int snsId, int projectId)
        {
            return Json(maintenancesManager.GetSnsCode(snsId, projectId));
        }

        public JsonResult GetIssueTypeFiles(int projectId)
        {
            List<IssueTypeFile> issues = new List<IssueTypeFile>();

            using (ApplicationDbContext applicationContext = new())
            {
                var project = applicationContext.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                if (project != null)
                {
                    issues = applicationContext.IssueTypeFiles.Where(p => p.IssueNoId == project.IssueNoId).ToList();
                }
            }

            return Json(issues);
        }

        public FileResult DownloadDMCTemplate()
        {
            StringBuilder csvHeader = new StringBuilder();
            csvHeader.Append("Project Name,");
            csvHeader.Append("Tech Name,");
            csvHeader.Append("Info Name,");
            csvHeader.Append("Schema File,");
            csvHeader.Append("SNS,");
            csvHeader.Append("DC,");
            csvHeader.Append("DCV,");
            csvHeader.Append("Information Code,");
            csvHeader.Append("ICV,");
            csvHeader.Append("Location Code");

            return File(Encoding.UTF8.GetBytes(csvHeader.ToString()), "text/plain", "DMCTemplate.csv");

            //using (var writer = new StreamWriter("DMCTemplate.csv"))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(myPersonObjects);
            //}

            //File.WriteAllText(filePath, csv.ToString());


            //DataTable dt = new DataTable("Grid");
            //dt.Columns.AddRange(new DataColumn[10] 
            //                        {
            //                            new DataColumn("Project Name"),
            //                            new DataColumn("Tech Name"),
            //                            new DataColumn("Info Name"),
            //                            new DataColumn("Schema File"),
            //                            new DataColumn("SNS"),
            //                            new DataColumn("DC"),
            //                            new DataColumn("DCV"),
            //                            new DataColumn("Information Code"),
            //                            new DataColumn("ICV"),
            //                            new DataColumn("Location Code"),
            //                        });

            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt);
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        wb.SaveAs(stream);
            //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DMC.xlsx");
            //    }
            //}
        }

        [HttpPost]
        public IActionResult UploadDMC()
        {
            List<string> successfulIds = new List<string>();
            List<string> failedIds = new List<string>();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
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
                            if (isHeaderCompleted == true)
                            {
                                try
                                {
                                    Project project = context.Projects.Where(p => p.Name == fields[0]).FirstOrDefault();
                                    InformationCode informationCode = context.InformationCodes.Where(i => i.Code == fields[7] && i.InformationCodeSetId == project.InformationCodeId).FirstOrDefault();
                                    LocationCode locationCode = context.LocationCodes.Where(l => l.Code == fields[9] && l.LocationCodeSetId == project.LocationCodeId).FirstOrDefault();
                                    IssueTypeFile issueTypeFile = context.IssueTypeFiles.Where(f => f.Name == fields[3]).FirstOrDefault();

                                    string dcValue = string.IsNullOrEmpty(fields[5]) ? "-" : "-" + fields[5];
                                    string dmCode = @"DMC -"
                                                        + project.ModelIdentification + "-"
                                                            + project.SDC + "-"
                                                            + fields[4]
                                                            + dcValue
                                                            + fields[6] + "-"
                                                            + informationCode.Code
                                                            + fields[8] + "-"
                                                            + locationCode.Code;

                                    context.DataModuleCodes.Add(new DataModuleCode()
                                    {
                                        CreatedBy = User.Identity.Name,
                                        CreatedOn = DateTime.UtcNow,
                                        DC = fields[5],
                                        DCV = fields[6],
                                        ICV = fields[8],
                                        InfoName = fields[2],
                                        InformationCodeId = informationCode.Id,
                                        IssueFileId = issueTypeFile.Id,
                                        LocationCodeId = locationCode.Id,
                                        ModelIdentification = project.ModelIdentification,
                                        ProjectId = project.Id,
                                        SDC = project.SDC,
                                        StandardNumberingSystem = fields[4],
                                        TechName = fields[1],
                                        DMC = dmCode
                                    });

                                    successfulIds.Add(String.Join(",", fields));

                                    context.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    failedIds.Add(String.Join(",", fields));
                                }
                            }
                            isHeaderCompleted = true;
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Maintenance");
        }

        public FileResult ViewDMCXml(int dmcId)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                DataModuleCode dmc = applicationContext.DataModuleCodes.Where(d => d.Id == dmcId).FirstOrDefault();
                return File(Encoding.UTF8.GetBytes(dmc.xml), "text/xml", dmc.DMC + ".xml");
            }
        }

        public JsonResult GenerateBrexXml(int projectId)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                DataModuleCode dmcCode = applicationContext.DataModuleCodes.Where(d => d.IsBrexXml == true && d.ProjectId == projectId).FirstOrDefault();
                if (dmcCode == null)
                {
                    Project project = applicationContext.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                    ResponsiblePartnerCode rpc = applicationContext.ResponsiblePartnerCodes.Where(r => r.Id == project.RPCId).FirstOrDefault();

                    string xmlContext = project.BrexTemplate;

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContext);

                    XmlNamespaceManager xMan = new XmlNamespaceManager(xmlDoc.NameTable);

                    StringBuilder dmcString = new StringBuilder();
                    dmcString.Append("DMC -");
                    dmcString.Append(project.ModelIdentification);
                    dmcString.Append("-");
                    dmcString.Append(project.SDC);
                    dmcString.Append("-");

                    string informationCodeString = string.Empty;
                    string locationCodeString = string.Empty;
                    string dcString = string.Empty;
                    string dcvString = string.Empty;
                    string icString = string.Empty;
                    string icvString = string.Empty;
                    string infoNameString = string.Empty;
                    string snsString = string.Empty;
                    string techNameString = string.Empty;

                    XmlNode node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/dmCode", xMan);
                    if (node != null)
                    {
                        dmcString.Append(node.Attributes["systemCode"].Value);
                        dmcString.Append("-");
                        dmcString.Append(node.Attributes["disassyCode"].Value);
                        dmcString.Append("-");
                        dmcString.Append(node.Attributes["disassyCodeVariant"].Value);
                        dmcString.Append("-");
                        dmcString.Append(node.Attributes["infoCode"].Value);
                        dmcString.Append("-");
                        dmcString.Append(node.Attributes["infoCodeVariant"].Value);
                        dmcString.Append("-");
                        dmcString.Append(node.Attributes["itemLocationCode"].Value);
                        dmcString.Append("-");

                        informationCodeString = node.Attributes["infoCode"].Value;
                        locationCodeString = node.Attributes["itemLocationCode"].Value;
                        snsString = node.Attributes["systemCode"].Value;

                        node.Attributes["modelIdentCode"].Value = project.ModelIdentification;
                        node.Attributes["subSubSystemCode"].Value = "0";
                        node.Attributes["subSystemCode"].Value = "0";
                        node.Attributes["systemDiffCode"].Value = project.SDC;
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/title", xMan);
                    if (node != null)
                    {
                        techNameString = node.InnerText;
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/subject", xMan);
                    if (node != null)
                    {
                        infoNameString = node.InnerText;
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/creator", xMan);
                    if (node != null)
                    {
                        node.InnerText = rpc.Rpccage;
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/publisher", xMan);
                    if (node != null)
                    {
                        node.InnerText = rpc.Rpccage;
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/contributor", xMan);
                    if (node != null)
                    {
                        node.InnerText = rpc.Rpccage;
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/date", xMan);
                    if (node != null)
                    {
                        node.InnerText = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/type", xMan);
                    if (node != null)
                    {
                        node.InnerText = "text";
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/format", xMan);
                    if (node != null)
                    {
                        node.InnerText = "text / xml";
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/identifier", xMan);
                    if (node != null)
                    {
                        node.InnerText = dmcString.ToString();
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/language", xMan);
                    if (node != null)
                    {
                        node.InnerText = "en-US";
                    }
                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/rights", xMan);
                    if (node != null)
                    {
                        node.InnerText = "01_cc51"; //need to check
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/language", xMan);
                    if (node != null)
                    {
                        node.Attributes["countryIsoCode"].Value = "US";
                        node.Attributes["languageIsoCode"].Value = "en";
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/issueInfo", xMan);
                    if (node != null)
                    {
                        node.Attributes["inWork"].Value = "00";
                        node.Attributes["issueNumber"].Value = "001";
                    }

                    node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/issueDate", xMan);
                    if (node != null)
                    {
                        node.Attributes["day"].Value = DateTime.Now.Date.Day.ToString("D2");
                        node.Attributes["month"].Value = DateTime.Now.Date.Month.ToString("D2");
                        node.Attributes["year"].Value = DateTime.Now.Date.Year.ToString("D4");
                    }

                    StringWriter stringWriter = new StringWriter();
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                    xmlDoc.WriteTo(xmlTextWriter);
                    InformationCode informationCode = applicationContext.InformationCodes.Where(i => i.Code == informationCodeString).FirstOrDefault();
                    LocationCode locationCode = applicationContext.LocationCodes.Where(i => i.Code == locationCodeString).FirstOrDefault();

                    DataModuleCode dataModuleCode = new DataModuleCode()
                    {
                        IsDeleted = false,
                        InformationCodeId = 0,
                        LocationCodeId = locationCode.Id,
                        CreatedBy = User.Identity.Name,
                        CreatedOn = DateTime.UtcNow,
                        DC = dcString,
                        DCV = dcvString,
                        DMC = dmcString.ToString(),
                        ICV = icvString,
                        InfoName = infoNameString,
                        IssueFileId = 1,
                        ModelIdentification = project.ModelIdentification,
                        ProjectId = project.Id,
                        SDC = project.SDC,
                        StandardNumberingSystem = snsString,
                        TechName = techNameString,
                        IsBrexXml = true,
                        xml = stringWriter.ToString()
                    };

                    applicationContext.DataModuleCodes.Add(dataModuleCode);
                    applicationContext.SaveChanges();
                    return (new JsonResult(new { Status = true, Message = "Brex XML Created Successfully!" }));
                }
                else
                {
                    return (new JsonResult(new { Status = false, Message = "Brex XML is already available for this Project!" }));
                }
            }
        }

        public JsonResult GenerateDMCXml(int dmcId)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                DataModuleCode dmc = applicationContext.DataModuleCodes.Where(d => d.Id == dmcId).FirstOrDefault();
                Project project = applicationContext.Projects.Where(p => p.Id == dmc.ProjectId).FirstOrDefault();
                ResponsiblePartnerCode rpc = applicationContext.ResponsiblePartnerCodes.Where(r => r.Id == project.RPCId).FirstOrDefault();
                InformationCode informationCode = applicationContext.InformationCodes.Where(i => i.Id == dmc.InformationCodeId).FirstOrDefault();
                LocationCode locationCode = applicationContext.LocationCodes.Where(l => l.Id == dmc.LocationCodeId).FirstOrDefault();

                string xmlContext = project.BrexTemplate;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContext);

                XmlNamespaceManager xMan = new XmlNamespaceManager(xmlDoc.NameTable);
                //xMan.AddNamespace("noNamespaceSchemaLocation", "http://www.s1000d.org/S1000D_4-0/xml_schema_flat/descript.xsd");
                //xMan.AddNamespace("dc", "http://www.purl.org/dc/elements/1.1/");
                //xMan.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                //xMan.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
                //xMan.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlNode node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/creator", xMan);
                if (node != null)
                {
                    node.InnerText = rpc.Rpccage;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/title", xMan);
                if (node != null)
                {
                    node.InnerText = dmc.TechName;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/subject", xMan);
                if (node != null)
                {
                    node.InnerText = dmc.InfoName;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/publisher", xMan);
                if (node != null)
                {
                    node.InnerText = rpc.Rpccage;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/contributor", xMan);
                if (node != null)
                {
                    node.InnerText = rpc.Rpccage;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/date", xMan);
                if (node != null)
                {
                    node.InnerText = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/type", xMan);
                if (node != null)
                {
                    node.InnerText = "text";
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/format", xMan);
                if (node != null)
                {
                    node.InnerText = "text / xml";
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/identifier", xMan);
                if (node != null)
                {
                    node.InnerText = dmc.DMC;
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/language", xMan);
                if (node != null)
                {
                    node.InnerText = "en-US";
                }
                node = xmlDoc.SelectSingleNode("/dmodule[@*]/Description/rights", xMan);
                if (node != null)
                {
                    node.InnerText = "01_cc51"; //need to check
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/dmCode", xMan);
                if (node != null)
                {
                    node.Attributes["assyCode"].Value = dmc.DC; //Need to check
                    node.Attributes["disassyCode"].Value = dmc.DC;
                    node.Attributes["disassyCodeVariant"].Value = dmc.DCV;
                    node.Attributes["infoCode"].Value = informationCode.Code;
                    node.Attributes["infoCodeVariant"].Value = dmc.ICV;
                    node.Attributes["itemLocationCode"].Value = locationCode.Code;
                    node.Attributes["modelIdentCode"].Value = dmc.ModelIdentification;
                    node.Attributes["subSubSystemCode"].Value = "0";
                    node.Attributes["subSystemCode"].Value = "0";
                    node.Attributes["systemCode"].Value = dmc.StandardNumberingSystem; //Need to check
                    node.Attributes["systemDiffCode"].Value = dmc.SDC;
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/language", xMan);
                if (node != null)
                {
                    node.Attributes["countryIsoCode"].Value = "US";
                    node.Attributes["languageIsoCode"].Value = "en";
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmIdent/issueInfo", xMan);
                if (node != null)
                {
                    node.Attributes["inWork"].Value = "00";
                    node.Attributes["issueNumber"].Value = "001";
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/issueDate", xMan);
                if (node != null)
                {
                    node.Attributes["day"].Value = DateTime.Now.Date.Day.ToString("D2");
                    node.Attributes["month"].Value = DateTime.Now.Date.Month.ToString("D2");
                    node.Attributes["year"].Value = DateTime.Now.Date.Year.ToString("D4");
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/dmTitle/techName", xMan);
                if (node != null)
                {
                    node.InnerText = dmc.TechName;
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmAddress/dmAddressItems/dmTitle/infoName", xMan);
                if (node != null)
                {
                    node.InnerText = dmc.InfoName;
                }

                node = xmlDoc.SelectSingleNode("/dmodule[@*]/identAndStatusSection/dmStatus/brexDmRef/dmRef/dmRefIdent/dmCode", xMan);
                if (node != null)
                {
                    node.Attributes["assyCode"].Value = dmc.DC; //Need to check
                    node.Attributes["disassyCode"].Value = dmc.DC;
                    node.Attributes["disassyCodeVariant"].Value = dmc.DCV;
                    node.Attributes["infoCode"].Value = informationCode.Code;
                    node.Attributes["infoCodeVariant"].Value = dmc.ICV;
                    node.Attributes["itemLocationCode"].Value = locationCode.Code;
                    node.Attributes["modelIdentCode"].Value = dmc.ModelIdentification;
                    node.Attributes["subSubSystemCode"].Value = "0";
                    node.Attributes["subSystemCode"].Value = "0";
                    node.Attributes["systemCode"].Value = dmc.StandardNumberingSystem; //Need to check
                    node.Attributes["systemDiffCode"].Value = dmc.SDC;
                }

                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                xmlDoc.WriteTo(xmlTextWriter);
                dmc.xml = stringWriter.ToString();

                applicationContext.SaveChanges();

                return new JsonResult(true);
            }


        }

        //public JsonResult GenerateDMCXml(int dmcId)
        //{
        //    using (ApplicationDbContext applicationContext = new())
        //    {
        //        DataModuleCode dmc = applicationContext.DataModuleCodes.Where(d => d.Id == dmcId).FirstOrDefault();
        //        IssueTypeFile issueTypeFile = applicationContext.IssueTypeFiles.Where(i => i.Id == dmc.IssueFileId).FirstOrDefault();

        //        using (var stream = new MemoryStream())
        //        {
        //            try
        //            {
        //                System.Text.UTF8Encoding ob = new UTF8Encoding();
        //                byte[] xsdData = ob.GetBytes(issueTypeFile.Data);
        //                stream.Write(xsdData, 0, xsdData.Length);
        //                stream.Seek(0, SeekOrigin.Begin);

        //                XmlReaderSettings settings = new XmlReaderSettings();
        //                settings.IgnoreWhitespace = true;

        //                using (var reader = XmlReader.Create(stream, settings))
        //                {
        //                    var schema = XmlSchema.Read(reader, null);
        //                    //var gen = new XmlSampleGenerator(schema, new XmlQualifiedName("dmodule"));

        //                    StringBuilder xml = new StringBuilder();

        //                    using (var writeStream = new MemoryStream())
        //                    {
        //                        XmlTextWriter textWriter = new XmlTextWriter(writeStream, null);
        //                        textWriter.Formatting = Formatting.Indented;
        //                        XmlQualifiedName qname = new XmlQualifiedName("dmodule");
        //                        XmlSampleGenerator generator = new XmlSampleGenerator(schema, qname);
        //                        generator.WriteXml(textWriter);
        //                        writeStream.Position = 0;

        //                        using (var streamReader = new StreamReader(writeStream))
        //                        {
        //                            string text = streamReader.ReadToEnd();
        //                        }
        //                    }
        //                }
        //            }   
        //            catch(Exception ex)
        //            {

        //            }

        //            return new JsonResult(true);
        //        }

        //    }


        //}
    }
}
