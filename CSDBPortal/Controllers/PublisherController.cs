using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.Services;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Text.Json;

namespace CSDBPortal.Controllers
{
    public class PublisherController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // Legacy "Publisher" claim grants access to all three tabs so that roles
            // created before the granular permissions were introduced keep working.
            var hasLegacy  = HasPermission(Features.Publisher);
            var canIetp    = hasLegacy || HasPermission(Features.PublisherIetp);
            var canPdf     = hasLegacy || HasPermission(Features.PublisherPdf);
            var canLicense = hasLegacy || HasPermission(Features.PublisherLicense);

            var vm = new PublisherViewModel
            {
                CanPublishIetp   = canIetp,
                CanExportPdf     = canPdf,
                CanManageLicense = canLicense
            };

            // Only load data for the tabs the user can actually see
            if (canIetp || canPdf)
            {
                vm.Projects = await _db.Projects
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }

            if (canIetp)
            {
                vm.PngLogos = await _db.ImageAssets
                    .Where(i => i.MimeType == "image/png")
                    .OrderBy(i => i.Name)
                    .Select(i => new ImageAsset
                    {
                        Id       = i.Id,
                        Name     = i.Name,
                        FileName = i.FileName,
                        MimeType = i.MimeType
                        // Data intentionally omitted; loaded on demand via /Manage/ViewImageAsset
                    })
                    .ToListAsync();
            }

            if (canLicense)
            {
                var providerKeySetting = await _db.ApplicationSettings
                    .FirstOrDefaultAsync(s => s.Key == NavLicenseService.ProviderKeySettingName);

                vm.ProviderKey   = providerKeySetting?.Value;
                vm.IetpLicenses  = await _db.IetpLicenses
                    .OrderByDescending(l => l.CreationTime)
                    .ToListAsync();
            }

            return View(vm);
        }

        /// <summary>
        /// Regenerates the portal's provider key. Invalidates all existing Secured packages.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RegenerateProviderKey()
        {
            if (!HasPermission(Features.Publisher) && !HasPermission(Features.PublisherLicense))
                return PermissionDenied();

            var licenseService = new NavLicenseService(_db);
            var newKey = await licenseService.RegenerateProviderKeyAsync();
            return Json(new { success = true, key = newKey });
        }

        /// <summary>
        /// Generates and downloads a PDF for the selected project using Apache FOP.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ExportPdf(int projectId, string status)
        {
            if (!HasPermission(Features.Publisher) && !HasPermission(Features.PublisherPdf))
                return PermissionDenied();

            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
                return Json(new { success = false, message = "Project not found." });

            var dataModules = await _db.DataModuleCodes
                .Where(d => d.ProjectId == projectId && !d.IsDeleted && !d.IsBrexXml)
                .OrderBy(d => d.DMC)
                .ToListAsync();

            if (!dataModules.Any())
                return Json(new { success = false, message = "No data modules found for this project." });

            bool isDraft = string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase);

            var pdfBytes = await PdfExportService.GeneratePdfAsync(project, dataModules, isDraft);

            var cleanName   = SanitiseFilename(project.Name ?? project.Title ?? "project");
            var suffix      = isDraft ? "_DRAFT" : "";
            var pdfFileName = $"{cleanName}{suffix}.pdf";

            return File(pdfBytes, "application/pdf", pdfFileName);
        }

        /// <summary>
        /// Saves a LicenseKey value against a published .nav package record.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SaveLicenseKey(int id, string? licenseKey)
        {
            if (!HasPermission(Features.Publisher) && !HasPermission(Features.PublisherLicense))
                return PermissionDenied();

            var record = await _db.IetpLicenses.FirstOrDefaultAsync(l => l.Id == id);
            if (record == null)
                return Json(new { success = false, message = "Record not found." });

            record.LicenseKey = string.IsNullOrWhiteSpace(licenseKey) ? null : licenseKey.Trim();
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }

        /// <summary>
        /// Generates and downloads a .nav IETP package for the selected project.
        /// The .nav file is a ZIP archive containing data module XML files,
        /// a navigation.xml site map, License.lic, optional cover page and logo, and metadata.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PublishIetp(
            int    projectId,
            string packageType,
            int?   logoId,
            string security,
            string status,
            string htmlSource,
            IFormFile? htmlFile,
            IFormFileCollection? assets)
        {
            if (!HasPermission(Features.Publisher) && !HasPermission(Features.PublisherIetp))
                return PermissionDenied();

            // ── Validate inputs ──────────────────────────────────────────────
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
                return Json(new { success = false, message = "Project not found." });

            var dataModules = await _db.DataModuleCodes
                .Where(d => d.ProjectId == projectId && !d.IsDeleted && !d.IsBrexXml)
                .OrderBy(d => d.DMC)
                .ToListAsync();

            if (!dataModules.Any())
                return Json(new { success = false, message = "No data modules found for this project." });

            // ── Sanitise package type and build output filename ──────────────
            var pkg      = (packageType ?? "PMC").Trim().ToUpper();
            if (pkg != "PMC" && pkg != "DDN") pkg = "PMC";
            var cleanName = SanitiseFilename(project.Name ?? project.Title ?? "project");
            var navFileName = $"{pkg}_{cleanName}.nav";

            // ── Build navigation.xml ─────────────────────────────────────────
            var navXml  = BuildNavigationXml(project, dataModules);

            // ── Licensing ────────────────────────────────────────────────────
            bool isSecured = string.Equals(security, "Secured", StringComparison.OrdinalIgnoreCase);
            bool isDraft   = string.Equals(status,   "Draft",   StringComparison.OrdinalIgnoreCase);

            var licenseService = new NavLicenseService(_db);

            // For Secured packages the ProviderKey is embedded in License.lic so the
            // Viewer can tie the package to this portal installation.
            string? providerKey = isSecured
                ? await licenseService.GetOrCreateProviderKeyAsync()
                : null;

            // Generate the License.lic binary
            var licenseBytes = licenseService.GenerateLicenseFile(isDraft, providerKey);

            // Persist the publish record (LicenseKey stays null until Viewer admin activates)
            await licenseService.UpsertLicenseRecordAsync(navFileName, isSecured, isDraft, providerKey);

            var metadata = new
            {
                builderVersion     = "1.0.0",
                createdDate        = DateTime.UtcNow,
                fileType           = isSecured ? "Secured" : "Unsecured",
                deliveryType       = isDraft   ? "Draft"   : "Final",
                isEncrypted        = true,
                encryptionAlgorithm= "AES-128-CBC",
                description        = $"NavIETM project: {project.Name}",
                customProperties   = new Dictionary<string, string>
                {
                    ["ProjectName"]     = project.Name ?? "",
                    ["LicenseRequired"] = isSecured.ToString(),
                    ["HasWatermark"]    = isDraft.ToString(),
                    ["PackageType"]     = pkg
                }
            };
            var metadataJson = JsonSerializer.Serialize(metadata,
                new JsonSerializerOptions { WriteIndented = true });

            // ── Package everything into a MemoryStream ZIP ───────────────────
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
            {
                // 1. Data module XML files (encrypted so the Viewer can open them)
                foreach (var dm in dataModules)
                {
                    if (string.IsNullOrWhiteSpace(dm.xml)) continue;
                    var entryName = $"{dm.DMC}.xml";
                    var entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);
                    using var ew = entry.Open();
                    var xmlContent = isDraft ? NavWatermarkService.WatermarkXml(dm.xml) : dm.xml;
                    var encrypted = NavXmlEncryptionService.EncryptXml(xmlContent);
                    await ew.WriteAsync(encrypted);
                }

                // 2. navigation.xml (encrypted — Viewer expects it encrypted)
                var navEntry = zip.CreateEntry("navigation.xml", CompressionLevel.Optimal);
                using (var ew = navEntry.Open())
                {
                    var encryptedNav = NavXmlEncryptionService.EncryptXml(navXml);
                    await ew.WriteAsync(encryptedNav);
                }

                // 3. nav_metadata.json
                var metaEntry = zip.CreateEntry("nav_metadata.json", CompressionLevel.Optimal);
                using (var ew = metaEntry.Open())
                    await ew.WriteAsync(Encoding.UTF8.GetBytes(metadataJson));

                // 4. License.lic — always required by the Viewer
                //    Secured  → file size > 16 bytes (trial flag + encrypted client key)
                //    Unsecured → file size ≤ 16 bytes (trial flag only)
                var licEntry = zip.CreateEntry("License.lic", CompressionLevel.Optimal);
                using (var ew = licEntry.Open())
                    await ew.WriteAsync(licenseBytes);

                // 5. Cover page HTML (optional)
                if (htmlSource == "upload" && htmlFile != null && htmlFile.Length > 0)
                {
                    var htmlEntry = zip.CreateEntry("custom/welcome.html", CompressionLevel.Optimal);
                    using var ew = htmlEntry.Open();
                    await htmlFile.CopyToAsync(ew);

                    // Associated assets (images, css, js)
                    if (assets != null)
                    {
                        foreach (var asset in assets)
                        {
                            var assetEntry = zip.CreateEntry(
                                $"custom/{Path.GetFileName(asset.FileName)}", CompressionLevel.Optimal);
                            using var ae = assetEntry.Open();
                            await asset.CopyToAsync(ae);
                        }
                    }
                }

                // 6. OEM Logo (optional)
                if (logoId.HasValue && logoId.Value > 0)
                {
                    var logoAsset = await _db.ImageAssets.FirstOrDefaultAsync(i => i.Id == logoId.Value);
                    if (!string.IsNullOrWhiteSpace(logoAsset?.Data))
                    {
                        // Data is base64 (may have a data-URI prefix)
                        var b64 = logoAsset.Data;
                        var commaIdx = b64.IndexOf(',');
                        if (commaIdx >= 0) b64 = b64[(commaIdx + 1)..];
                        var logoBytes = Convert.FromBase64String(b64);
                        var logoEntry = zip.CreateEntry("custom/logo_right.png", CompressionLevel.Optimal);
                        using var ew = logoEntry.Open();
                        await ew.WriteAsync(logoBytes);
                    }
                }
            }

            ms.Position = 0;
            return File(ms.ToArray(), "application/octet-stream", navFileName);
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        /// <summary>Builds the navigation.xml siteMap from the project's data modules.</summary>
        private static string BuildNavigationXml(Project project, List<DataModuleCode> modules)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<!--Arbortext, Inc., 1988-2018, v.4002-->");
            sb.AppendLine("<siteMap>");

            string rootTitle = SecurityElement.Escape(project.Title ?? project.Name ?? "IETP");
            // Root node points to the first data module
            string firstUrl  = modules.Any()
                ? SecurityElement.Escape($"~/frames/IETP.aspx?DM={modules[0].DMC}.xml")
                : "#";
            sb.AppendLine($"    <siteMapNode title=\"{rootTitle}\" url=\"{firstUrl}\">");

            foreach (var dm in modules)
            {
                if (string.IsNullOrWhiteSpace(dm.DMC)) continue;
                string title = SecurityElement.Escape(
                    !string.IsNullOrWhiteSpace(dm.TechName) ? dm.TechName :
                    !string.IsNullOrWhiteSpace(dm.InfoName) ? dm.InfoName :
                    dm.DMC);
                string url = SecurityElement.Escape($"~/frames/IETP.aspx?DM={dm.DMC}.xml");
                sb.AppendLine($"        <siteMapNode title=\"{title}\" url=\"{url}\" />");
            }

            sb.AppendLine("    </siteMapNode>");
            sb.AppendLine("</siteMap>");
            return sb.ToString();
        }

        /// <summary>Removes characters that are invalid in filenames.</summary>
        private static string SanitiseFilename(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_').Replace('.', '_').Trim('_');
        }
    }
}
