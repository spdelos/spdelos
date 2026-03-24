using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;

namespace CSDBPortal.Controllers
{
    public class TaskController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly BrexValidationEngine _brexValidationEngine;
        private readonly IWebHostEnvironment _env;

        public TaskController(
            ApplicationDbContext db,
            BrexValidationEngine brexValidationEngine,
            IWebHostEnvironment env)
        {
            _db = db;
            _brexValidationEngine = brexValidationEngine;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var userName = User.Identity!.Name;

            var dmcs = await _db.DataModuleCodes
                .Where(d => d.AssignedTo == userName && !d.IsDeleted && !d.IsBrexXml)
                .Join(_db.Projects,
                    d => d.ProjectId,
                    p => p.Id,
                    (d, p) => new TaskDmcItem
                    {
                        Id = d.Id,
                        DMC = d.DMC,
                        ProjectName = p.Name,
                        CheckoutStatus = d.CheckoutStatus,
                        CheckedOutBy = d.CheckedOutBy,
                        CheckedOutOn = d.CheckedOutOn,
                        HasXml = d.xml != null
                    })
                .ToListAsync();

            var icns = await _db.IcnNumbers
                .Where(i => i.AssignedTo == userName)
                .Join(_db.Projects,
                    i => i.ProjectId,
                    p => p.Id,
                    (i, p) => new TaskIcnItem
                    {
                        Id = i.Id,
                        Number = i.Number,
                        ProjectName = p.Name,
                        ImagePath = i.ImagePath,
                        CheckedInBy = i.CheckedInBy,
                        CheckedInOn = i.CheckedInOn
                    })
                .ToListAsync();

            return View(new TaskViewModel { AssignedDmcs = dmcs, AssignedIcns = icns });
        }

        [HttpPost]
        public async Task<JsonResult> CheckoutDMC(int dmcId)
        {
            var userName = User.Identity!.Name;
            var dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null)
                return Json(new { status = false, message = "DMC not found." });
            if (dmc.AssignedTo != userName)
                return Json(new { status = false, message = "This DMC is not assigned to you." });
            if (dmc.CheckoutStatus == "CheckedOut")
                return Json(new { status = false, message = $"Already checked out by {dmc.CheckedOutBy}." });

            dmc.CheckoutStatus = "CheckedOut";
            dmc.CheckedOutBy = userName;
            dmc.CheckedOutOn = DateTime.UtcNow;
            dmc.OriginalXml = dmc.xml;
            await _db.SaveChangesAsync();

            return Json(new { status = true });
        }

        public async Task<FileResult> DownloadDMCXml(int dmcId)
        {
            var dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            return File(Encoding.UTF8.GetBytes(dmc!.xml!), "text/xml", dmc.DMC + ".xml");
        }

        [HttpPost]
        public async Task<JsonResult> CheckinDMC(int dmcId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { status = false, message = "No file uploaded." });

            var dmc = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == dmcId);
            if (dmc == null)
                return Json(new { status = false, message = "DMC not found." });
            if (dmc.CheckoutStatus != "CheckedOut")
                return Json(new { status = false, message = "This DMC is not currently checked out." });
            if (dmc.CheckedOutBy != User.Identity!.Name)
                return Json(new { status = false, message = "You did not check out this DMC." });

            string uploadedXml;
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                uploadedXml = await reader.ReadToEndAsync();

            var uploadedDoc = new XmlDocument();
            try { uploadedDoc.LoadXml(uploadedXml); }
            catch (Exception ex)
                { return Json(new { status = false, message = $"Uploaded file is not valid XML: {ex.Message}" }); }

            // Constraint 1 — identAndStatusSection must be unchanged
            if (!string.IsNullOrEmpty(dmc.OriginalXml))
            {
                try
                {
                    var originalDoc = new XmlDocument();
                    originalDoc.LoadXml(dmc.OriginalXml);
                    string origSection = NormalizeXml(originalDoc.SelectSingleNode("//identAndStatusSection")?.OuterXml ?? string.Empty);
                    string newSection  = NormalizeXml(uploadedDoc.SelectSingleNode("//identAndStatusSection")?.OuterXml ?? string.Empty);
                    if (!string.Equals(origSection, newSection, StringComparison.Ordinal))
                        return Json(new { status = false, message = "Check-in rejected: the identAndStatusSection has been modified." });
                }
                catch { /* OriginalXml unparseable — skip comparison */ }
            }

            // Constraint 2 — BREX validation
            var (brexPassed, brexMessage) = await _brexValidationEngine.ValidateXmlStringAsync(dmc.ProjectId, uploadedXml);
            if (!brexPassed)
                return Json(new { status = false, message = $"Check-in rejected: BREX validation failed. {brexMessage}" });

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

        [HttpPost]
        public async Task<JsonResult> CheckinICN(int icnId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { status = false, message = "No file uploaded." });

            var icn = await _db.IcnNumbers.FirstOrDefaultAsync(i => i.Id == icnId);
            if (icn == null)
                return Json(new { status = false, message = "ICN not found." });
            if (icn.AssignedTo != User.Identity!.Name)
                return Json(new { status = false, message = "This ICN is not assigned to you." });

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "icn");
            Directory.CreateDirectory(uploadsDir);
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{icn.Number}_{DateTime.UtcNow:yyyyMMddHHmmss}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            icn.ImagePath   = $"/uploads/icn/{fileName}";
            icn.CheckedInBy = User.Identity.Name;
            icn.CheckedInOn = DateTime.UtcNow;
            icn.UpdatedBy   = User.Identity.Name;
            icn.UpdatedOn   = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Json(new { status = true, message = "Check-in successful." });
        }

        private static string NormalizeXml(string xml) =>
            System.Text.RegularExpressions.Regex.Replace(xml, @"\s+", " ").Trim();
    }
}
