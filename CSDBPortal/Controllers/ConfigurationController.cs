using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

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
                List<DataModuleType> existingDMTypes = new();

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
                    existingDMTypes = await _db.DataModuleTypes.Where(d => d.IssueNo == issueNoId).ToListAsync();
                }

                issueNo.Name = Request.Form["txtIssueNo"];

                // Handle BREX XML file upload
                var brexFile = Request.Form.Files["brexFile"];
                if (brexFile != null && brexFile.Length > 0)
                {
                    var fileContent = new StringBuilder();
                    using (var reader = new StreamReader(brexFile.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                            fileContent.AppendLine(await reader.ReadLineAsync());
                    }
                    issueNo.BrexTemplate = fileContent.ToString();
                }

                // Save IssueNo first so we have its Id for FK references
                if (issueNoId <= 0)
                    _db.IssueNos.Add(issueNo);
                await _db.SaveChangesAsync();

                // Process schema file types submitted as JSON
                string schemaTypesJson = Request.Form["schemaTypes"];
                if (!string.IsNullOrEmpty(schemaTypesJson))
                {
                    var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var submittedTypes = JsonSerializer.Deserialize<List<SchemaTypeDto>>(schemaTypesJson, opts) ?? new();

                    // In edit mode: remove DataModuleTypes no longer in the submitted list
                    var submittedFileNames = submittedTypes.Select(t => t.FileName).ToHashSet(StringComparer.OrdinalIgnoreCase);
                    foreach (var existing in existingDMTypes)
                    {
                        if (!submittedFileNames.Contains(existing.FileName))
                            _db.DataModuleTypes.Remove(existing);
                    }

                    foreach (var typeDto in submittedTypes)
                    {
                        var existingDMT = existingDMTypes.FirstOrDefault(d =>
                            string.Equals(d.FileName, typeDto.FileName, StringComparison.OrdinalIgnoreCase));

                        if (existingDMT != null)
                        {
                            // Update name if changed
                            existingDMT.Name = typeDto.Name;
                            existingDMT.UpdatedBy = User.Identity.Name;
                            existingDMT.UpdatedOn = DateTime.UtcNow;
                            _db.Entry(existingDMT).State = EntityState.Modified;
                        }
                        else
                        {
                            // New schema type: create DataModuleType record
                            _db.DataModuleTypes.Add(new DataModuleType
                            {
                                Name = typeDto.Name,
                                FileName = typeDto.FileName,
                                IssueNo = issueNo.Id,
                                CreatedBy = User.Identity.Name,
                                CreatedOn = DateTime.UtcNow,
                                UpdatedBy = User.Identity.Name,
                                UpdatedOn = DateTime.UtcNow
                            });

                            // Create blank S1000D XML template as IssueTypeFile
                            issueNo.IssueTypeFiles.Add(new IssueTypeFile
                            {
                                Name = typeDto.FileName,
                                IssueNoId = issueNo.Id,
                                CreatedBy = User.Identity.Name,
                                CreateOn = DateTime.UtcNow,
                                Data = GenerateBlankDmoduleXml(typeDto.FileName)
                            });
                        }
                    }
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Configuration");
            }
            catch
            {
                return View("failed");
            }
        }

        private static string GenerateBlankDmoduleXml(string xsdFileName) =>
            $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <dmodule xmlns:dc="http://www.purl.org/dc/elements/1.1/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://www.s1000d.org/S1000D_4-2/xml_schema_flat/{xsdFileName}">
              <Description>
                <creator></creator>
                <title></title>
                <subject></subject>
                <publisher></publisher>
                <contributor></contributor>
                <date></date>
                <type></type>
                <format></format>
                <identifier></identifier>
                <language></language>
                <rights></rights>
              </Description>
              <identAndStatusSection>
                <dmAddress>
                  <dmIdent>
                    <language countryIsoCode="" languageIsoCode=""/>
                    <issueInfo inWork="" issueNumber=""/>
                  </dmIdent>
                  <dmAddressItems>
                    <issueDate day="" month="" year=""/>
                    <dmTitle>
                      <techName></techName>
                      <infoName></infoName>
                    </dmTitle>
                  </dmAddressItems>
                </dmAddress>
                <dmStatus>
                  <brexDmRef>
                    <dmRef>
                      <dmRefIdent>
                        <dmCode assyCode="" disassyCode="" disassyCodeVariant="" infoCode="" infoCodeVariant="" itemLocationCode="" modelIdentCode="" subSubSystemCode="" subSystemCode="" systemCode="" systemDiffCode=""/>
                      </dmRefIdent>
                    </dmRef>
                  </brexDmRef>
                </dmStatus>
              </identAndStatusSection>
            </dmodule>
            """;

        private sealed class SchemaTypeDto
        {
            public string Name { get; set; } = "";
            public string FileName { get; set; } = "";
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
