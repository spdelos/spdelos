using ClosedXML.Excel;
using CSDBPortal.Data;
using CSDBPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Controllers
{
    public class PNCController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public PNCController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index() => View();

        // ─── GET: lookup rows by type ────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetLookups(string type)
        {
            try
            {
                var rows = await _db.PNCLookups
                    .Where(l => l.LookupType == type)
                    .OrderBy(l => l.SortOrder).ThenBy(l => l.Code)
                    .Select(l => new { l.Id, l.Code, l.Description, l.SortOrder })
                    .ToListAsync();
                return Json(new { success = true, data = rows });
            }
            catch
            {
                return Json(new { success = true, data = Array.Empty<object>() });
            }
        }

        // ─── POST: save (insert or update) a lookup row ──────────────────────
        [HttpPost]
        public async Task<JsonResult> SaveLookup([FromBody] PNCLookup row)
        {
            if (!User.HasClaim("Permission", Features.UserAndRoleAdministration))
                return Json(new { success = false, message = "Administrator privileges are required to modify reference tables." });

            if (row == null || string.IsNullOrWhiteSpace(row.Code) || string.IsNullOrWhiteSpace(row.Description))
                return Json(new { success = false, message = "Code and Description are required." });

            row.Code        = row.Code.Trim().ToUpper();
            row.Description = row.Description.Trim();
            row.LookupType  = row.LookupType?.Trim() ?? "";

            try
            {
                // Check for duplicate code within same type (excluding self on update)
                bool dup = await _db.PNCLookups.AnyAsync(l =>
                    l.LookupType == row.LookupType && l.Code == row.Code && l.Id != row.Id);
                if (dup)
                    return Json(new { success = false, message = $"Code '{row.Code}' already exists in this table." });

                if (row.Id == 0)
                {
                    row.CreatedBy = User.Identity?.Name;
                    row.CreatedOn = DateTime.UtcNow;
                    _db.PNCLookups.Add(row);
                }
                else
                {
                    var existing = await _db.PNCLookups.FindAsync(row.Id);
                    if (existing == null) return Json(new { success = false, message = "Row not found." });
                    existing.Code        = row.Code;
                    existing.Description = row.Description;
                    existing.SortOrder   = row.SortOrder;
                }
                await _db.SaveChangesAsync();
                return Json(new { success = true, id = row.Id == 0 ? 0 : row.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database error: " + ex.Message });
            }
        }

        // ─── DELETE: remove a lookup row ─────────────────────────────────────
        [HttpDelete]
        public async Task<JsonResult> DeleteLookup(int id)
        {
            if (!User.HasClaim("Permission", Features.UserAndRoleAdministration))
                return Json(new { success = false, message = "Administrator privileges are required to modify reference tables." });

            try
            {
                var row = await _db.PNCLookups.FindAsync(id);
                if (row == null) return Json(new { success = false, message = "Not found." });
                _db.PNCLookups.Remove(row);
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database error: " + ex.Message });
            }
        }

        // ─── GET: distinct Model IDs ─────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetModelIds()
        {
            try
            {
                var ids = await _db.PartNumberCodes
                    .Select(p => p.ModelId)
                    .Distinct()
                    .OrderBy(m => m)
                    .ToListAsync();
                return Json(new { success = true, data = ids });
            }
            catch
            {
                // Table may not exist yet (pending migration) — return empty list
                return Json(new { success = true, data = new List<string>(), dbReady = false });
            }
        }

        // ─── GET: next available SeqDigits (scoped to model + sub-assembly) ──
        [HttpGet]
        public async Task<JsonResult> GetNextSeqDigits(string modelId, string? subAsmCode = null)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                return Json(new { success = false });
            try
            {
                var q = _db.PartNumberCodes
                    .Where(p => p.ModelId == modelId.Trim().ToUpper());

                if (!string.IsNullOrWhiteSpace(subAsmCode))
                    q = q.Where(p => p.SubAsmCode == subAsmCode.Trim().ToUpper());

                var max = await q.MaxAsync(p => (string?)p.SeqDigits);

                int next = 1;
                if (max != null && int.TryParse(max, out int parsed))
                    next = parsed + 1;

                return Json(new { success = true, nextSeq = next.ToString("D5") });
            }
            catch
            {
                return Json(new { success = true, nextSeq = "00001" });
            }
        }

        // ─── GET: filtered list ──────────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetPNCs(
            string? seg1, string? seg2, string? seg3, string? seg4,
            string? segDOR, string? segDSN,
            string? seg5, string? seg6, string? seg7, bool includeObsolete = true)
        {
            try
            {
                var q = _db.PartNumberCodes.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(seg1))   q = q.Where(p => p.ModelId      == seg1.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg2))   q = q.Where(p => p.EqCode       == seg2.Trim());
                if (!string.IsNullOrWhiteSpace(seg3))   q = q.Where(p => p.ModCode      == seg3.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg4))   q = q.Where(p => p.SubAsmCode   == seg4.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(segDOR)) q = q.Where(p => p.DesignOffice == segDOR.Trim());
                if (!string.IsNullOrWhiteSpace(segDSN)) q = q.Where(p => p.DrawingSeqNo == segDSN.Trim());
                if (!string.IsNullOrWhiteSpace(seg5))   q = q.Where(p => p.SeqDigits    == seg5.Trim());
                if (!string.IsNullOrWhiteSpace(seg6))   q = q.Where(p => p.MaintLevel   == seg6.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg7))   q = q.Where(p => p.RevSuffix    == seg7.Trim().ToUpper());
                if (!includeObsolete)                   q = q.Where(p => !p.IsObsolete);

                var rows = await q.OrderBy(p => p.FullPNC)
                                  .Select(p => new {
                                      p.Id, p.ModelId, p.EqCode, p.ModCode, p.SubAsmCode,
                                      p.DesignOffice, p.DrawingSeqNo,
                                      p.SeqDigits, p.MaintLevel, p.RevSuffix, p.FullPNC, p.PartName,
                                      p.IsObsolete, p.ObsoletedBy,
                                      ObsoletedOn = p.ObsoletedOn.HasValue
                                          ? p.ObsoletedOn.Value.ToString("yyyy-MM-dd HH:mm") : null,
                                      p.CreatedBy,
                                      CreatedOn = p.CreatedOn.ToString("yyyy-MM-dd HH:mm")
                                  })
                                  .ToListAsync();

                return Json(new { success = true, data = rows });
            }
            catch
            {
                // Table may not exist yet (pending migration) — return empty list
                return Json(new { success = true, data = Array.Empty<object>() });
            }
        }

        // ─── POST: save single PNC ───────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> SavePNC([FromBody] PartNumberCode pnc)
        {
            if (pnc == null)
                return Json(new { success = false, message = "No data received." });

            pnc.ModelId      = (pnc.ModelId      ?? "").Trim().ToUpper();
            pnc.EqCode       = (pnc.EqCode       ?? "").Trim();
            pnc.ModCode      = (pnc.ModCode      ?? "").Trim().ToUpper();
            pnc.SubAsmCode   = (pnc.SubAsmCode   ?? "").Trim().ToUpper();
            pnc.DesignOffice = (pnc.DesignOffice ?? "").Trim();
            pnc.DrawingSeqNo = (pnc.DrawingSeqNo ?? "").Trim();
            pnc.SeqDigits    = (pnc.SeqDigits    ?? "").Trim();
            pnc.MaintLevel   = (pnc.MaintLevel   ?? "").Trim().ToUpper();
            pnc.RevSuffix    = (pnc.RevSuffix    ?? "").Trim().ToUpper();
            pnc.PartName     = (pnc.PartName     ?? "").Trim();
            pnc.FullPNC      = $"{pnc.ModelId}-{pnc.EqCode}-{pnc.ModCode}-{pnc.SubAsmCode}-{pnc.DesignOffice}-{pnc.DrawingSeqNo}-{pnc.SeqDigits}-{pnc.MaintLevel}-{pnc.RevSuffix}";
            pnc.CreatedBy  = User.Identity?.Name;
            pnc.CreatedOn  = DateTime.UtcNow;
            pnc.IsObsolete = false;

            var msg = ValidatePNC(pnc);
            if (msg != null) return Json(new { success = false, message = msg });

            try
            {
                bool exists = await _db.PartNumberCodes.AnyAsync(p => p.FullPNC == pnc.FullPNC);
                if (exists)
                    return Json(new { success = false, message = $"PNC '{pnc.FullPNC}' already exists.", duplicate = true });

                _db.PartNumberCodes.Add(pnc);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = $"PNC '{pnc.FullPNC}' saved.", id = pnc.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database error: " + ex.Message });
            }
        }

        // ─── POST: mark obsolete ─────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> MarkObsolete(int id)
        {
            try
            {
                var pnc = await _db.PartNumberCodes.FindAsync(id);
                if (pnc == null) return Json(new { success = false, message = "Not found." });
                if (pnc.IsObsolete) return Json(new { success = false, message = "Already deactivated." });

                pnc.IsObsolete  = true;
                pnc.ObsoletedBy = User.Identity?.Name;
                pnc.ObsoletedOn = DateTime.UtcNow;
                await _db.SaveChangesAsync();
                return Json(new { success = true, fullPNC = pnc.FullPNC });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database error: " + ex.Message });
            }
        }

        // ─── GET: download filtered results as Excel ─────────────────────────
        [HttpGet]
        public async Task<IActionResult> DownloadFiltered(
            string? seg1, string? seg2, string? seg3, string? seg4,
            string? segDOR, string? segDSN,
            string? seg5, string? seg6, string? seg7, bool includeObsolete = true)
        {
            var q = _db.PartNumberCodes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(seg1))   q = q.Where(p => p.ModelId      == seg1.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg2))   q = q.Where(p => p.EqCode       == seg2.Trim());
            if (!string.IsNullOrWhiteSpace(seg3))   q = q.Where(p => p.ModCode      == seg3.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg4))   q = q.Where(p => p.SubAsmCode   == seg4.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(segDOR)) q = q.Where(p => p.DesignOffice == segDOR.Trim());
            if (!string.IsNullOrWhiteSpace(segDSN)) q = q.Where(p => p.DrawingSeqNo == segDSN.Trim());
            if (!string.IsNullOrWhiteSpace(seg5))   q = q.Where(p => p.SeqDigits    == seg5.Trim());
            if (!string.IsNullOrWhiteSpace(seg6))   q = q.Where(p => p.MaintLevel   == seg6.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg7))   q = q.Where(p => p.RevSuffix    == seg7.Trim().ToUpper());
            if (!includeObsolete)                   q = q.Where(p => !p.IsObsolete);

            var rows = await q.OrderBy(p => p.FullPNC).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Part Numbers");

            // Header
            string[] headers = { "ModelId", "Chapter (EqCode)", "Section (ModCode)", "Sub Sec (SubAsmCode)",
                                  "Design Office", "Drawing Seq. No.",
                                  "Seq. No.", "Tech Spec No.", "Colour", "Full PNS", "Part Name",
                                  "Status", "Created By", "Created On",
                                  "DeActivated By", "DeActivated On" };
            for (int c = 0; c < headers.Length; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value = headers[c];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            int r = 2;
            foreach (var p in rows)
            {
                ws.Cell(r, 1).Value  = p.ModelId;
                ws.Cell(r, 2).Value  = p.EqCode;
                ws.Cell(r, 3).Value  = p.ModCode;
                ws.Cell(r, 4).Value  = p.SubAsmCode;
                ws.Cell(r, 5).Value  = p.DesignOffice;
                ws.Cell(r, 6).Value  = p.DrawingSeqNo;
                ws.Cell(r, 7).Value  = p.SeqDigits;
                ws.Cell(r, 8).Value  = p.MaintLevel;
                ws.Cell(r, 9).Value  = p.RevSuffix;
                ws.Cell(r, 10).Value = p.FullPNC;
                ws.Cell(r, 11).Value = p.PartName    ?? "";
                ws.Cell(r, 12).Value = p.IsObsolete ? "DeActivated" : "Active";
                ws.Cell(r, 13).Value = p.CreatedBy   ?? "";
                ws.Cell(r, 14).Value = p.CreatedOn.ToString("yyyy-MM-dd HH:mm");
                ws.Cell(r, 15).Value = p.ObsoletedBy ?? "";
                ws.Cell(r, 16).Value = p.ObsoletedOn.HasValue
                    ? p.ObsoletedOn.Value.ToString("yyyy-MM-dd HH:mm") : "";

                if (p.IsObsolete)
                {
                    var row = ws.Row(r);
                    row.Style.Font.Strikethrough = true;
                    row.Style.Font.FontColor = XLColor.Gray;
                }
                r++;
            }

            ws.Columns().AdjustToContents();

            // Auto-filter on header row
            ws.RangeUsed()!.SetAutoFilter();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;

            var fileName = $"PNC_Export_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx";
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        // ─── GET: download blank template ────────────────────────────────────
        [HttpGet]
        public IActionResult DownloadTemplate()
        {
            using var wb = new XLWorkbook();

            // ── Main data sheet ──────────────────────────────────────────────
            var ws = wb.Worksheets.Add("PNS_Template");

            // Column headers must match UploadExcel expectedHeaders exactly
            string[] headers = {
                "ModelId", "EqCode", "ModCode", "SubAsmCode",
                "DesignOffice", "DrawingSeqNo",
                "SeqDigits", "MaintLevel", "RevSuffix", "PartName"
            };
            string[] friendlyHeaders = {
                "Model ID", "Chapter (EqCode)", "Section (ModCode)", "Sub Sec (SubAsmCode)",
                "Design Office (DesignOffice)", "Drawing Seq. No. (DrawingSeqNo)",
                "Seq. No. (SeqDigits)", "Tech Spec No. (MaintLevel)", "Colour (RevSuffix)",
                "Part Name (PartName)"
            };
            for (int c = 0; c < headers.Length; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value = headers[c];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                // Friendly name as a comment
                ws.Cell(2, c + 1).Value = friendlyHeaders[c];
                ws.Cell(2, c + 1).Style.Font.Italic = true;
                ws.Cell(2, c + 1).Style.Font.FontColor = XLColor.Gray;
            }

            // Example data row (row 3)
            ws.Cell(3, 1).Value = "KAV";
            ws.Cell(3, 2).Value = "72";
            ws.Cell(3, 3).Value = "FAN";
            ws.Cell(3, 4).Value = "FCA";
            ws.Cell(3, 5).Value = "DO1";   // Design Office code (from Design Offices table)
            ws.Cell(3, 6).Value = "001";   // Drawing Sequential Number (3 digits)
            ws.Cell(3, 7).Value = "00001"; // Seq. No. (5 digits)
            ws.Cell(3, 8).Value = "01";    // Tech Spec No. (2 digits)
            ws.Cell(3, 9).Value  = "A";                // Colour Scheme (single letter A-Z)
            ws.Cell(3, 10).Value = "e.g. Fan Blade";   // Part Name (free text)
            for (int c = 1; c <= 10; c++)
                ws.Cell(3, c).Style.Font.Italic = true;

            ws.Columns().AdjustToContents();

            // ── Reference sheet: Chapters (EqCode) ──────────────────────────
            var wsEq = wb.Worksheets.Add("Chapters (EqCode)");
            wsEq.Cell(1, 1).Value = "Code"; wsEq.Cell(1, 2).Value = "Description";
            wsEq.Cell(1, 1).Style.Font.Bold = true; wsEq.Cell(1, 2).Style.Font.Bold = true;
            wsEq.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            wsEq.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
            string[,] eqData = {
                {"70","Standard Practices"},{"71","Power Plant"},{"72","Engine"},
                {"73","Engine Fuel and Control"},{"74","Ignition"},{"75","Air General"},
                {"76","Engine Controls"},{"77","Engine Indicating"},{"78","Exhaust"},
                {"79","Oil"},{"80","Starting"}
            };
            for (int i = 0; i < eqData.GetLength(0); i++)
            {
                wsEq.Cell(i + 2, 1).Value = eqData[i, 0];
                wsEq.Cell(i + 2, 2).Value = eqData[i, 1];
            }
            wsEq.Columns().AdjustToContents();

            // ── Reference sheet: Sections (ModCode) ─────────────────────────
            var wsMod = wb.Worksheets.Add("Sections (ModCode)");
            wsMod.Cell(1, 1).Value = "Code"; wsMod.Cell(1, 2).Value = "Description";
            wsMod.Cell(1, 1).Style.Font.Bold = true; wsMod.Cell(1, 2).Style.Font.Bold = true;
            wsMod.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            wsMod.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
            string[,] modData = {
                {"EGN","Engine General"},{"EEX","Engine Exhaust"},{"LPC","Low Pressure Compressor"},
                {"COU","Coupler"},{"FAN","Fan"},{"ICA","Intermediate Casing"},
                {"HPC","High Pressure Compressor"},{"DCO","Diffuser & Combustor"},{"TNZ","Transition Zone"},
                {"HPT","High Pressure Turbine"},{"LPT","Low Pressure Turbine"},{"TEC","Turbine Exhaust Case"},
                {"MGB","Main Gearbox"},{"AGB","Accessory Gearbox"}
            };
            for (int i = 0; i < modData.GetLength(0); i++)
            {
                wsMod.Cell(i + 2, 1).Value = modData[i, 0];
                wsMod.Cell(i + 2, 2).Value = modData[i, 1];
            }
            wsMod.Columns().AdjustToContents();

            // ── Reference sheet: Sub Sections (SubAsmCode) ──────────────────
            var wsSub = wb.Worksheets.Add("Sub Sections (SubAsmCode)");
            wsSub.Cell(1, 1).Value = "Code"; wsSub.Cell(1, 2).Value = "Description";
            wsSub.Cell(1, 1).Style.Font.Bold = true; wsSub.Cell(1, 2).Style.Font.Bold = true;
            wsSub.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            wsSub.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
            string[,] subData = {
                {"FCA","Fan Case"},{"FBL","Fan Blades"},{"FHB","Fan Hub"},{"FSH","Fan Shaft"},
                {"BRG","Bearings"},{"TTG","Test Tools & Ground Equipment (TT&GE)"},
                {"FDU","Fan Duct"},{"FVA","Fan Vane"},{"POL","POL (Petroleum, Oil, Lubricants)"},
                {"STP","Standard Fasteners (Bolts, Nuts, Washers)"},
                {"CON","Consumables (Class C items)"},{"SPR","Spares (Replacement Kits)"}
            };
            for (int i = 0; i < subData.GetLength(0); i++)
            {
                wsSub.Cell(i + 2, 1).Value = subData[i, 0];
                wsSub.Cell(i + 2, 2).Value = subData[i, 1];
            }
            wsSub.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PNS_Template.xlsx");
        }

        // ─── POST: upload Excel ──────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "No file received." });

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".xlsx" && ext != ".xls")
                return Json(new { success = false, message = "Only .xlsx files are supported." });

            var imported  = new List<string>();
            var duplicates = new List<string>();
            var errors    = new List<string>();

            try
            {
                using var stream = file.OpenReadStream();
                using var wb = new XLWorkbook(stream);
                var ws = wb.Worksheets.First();

                // Validate header row matches template columns
                string[] expectedHeaders = { "ModelId", "EqCode", "ModCode", "SubAsmCode",
                                             "DesignOffice", "DrawingSeqNo",
                                             "SeqDigits", "MaintLevel", "RevSuffix", "PartName" };
                for (int c = 0; c < expectedHeaders.Length; c++)
                {
                    var h = ws.Cell(1, c + 1).GetString().Trim();
                    if (!h.StartsWith(expectedHeaders[c], StringComparison.OrdinalIgnoreCase))
                        return Json(new { success = false, message = $"Incompatible format — column {c+1} expected '{expectedHeaders[c]}', found '{h}'. Use the PNC template." });
                }

                // Template layout: row 1 = machine headers, row 2 = friendly names, row 3 = example
                int row = 4;
                while (true)
                {
                    var cell = ws.Cell(row, 1).GetString().Trim();
                    if (string.IsNullOrEmpty(cell) && row > 4) break;
                    if (string.IsNullOrEmpty(cell)) { row++; continue; }

                    var pnc = new PartNumberCode
                    {
                        ModelId      = ws.Cell(row, 1).GetString().Trim().ToUpper(),
                        EqCode       = ws.Cell(row, 2).GetString().Trim(),
                        ModCode      = ws.Cell(row, 3).GetString().Trim().ToUpper(),
                        SubAsmCode   = ws.Cell(row, 4).GetString().Trim().ToUpper(),
                        DesignOffice = ws.Cell(row, 5).GetString().Trim(),
                        DrawingSeqNo = ws.Cell(row, 6).GetString().Trim(),
                        SeqDigits    = ws.Cell(row, 7).GetString().Trim(),
                        MaintLevel   = ws.Cell(row, 8).GetString().Trim().ToUpper(),
                        RevSuffix    = ws.Cell(row, 9).GetString().Trim().ToUpper(),
                        PartName     = ws.Cell(row, 10).GetString().Trim(),
                        CreatedBy    = User.Identity?.Name,
                        CreatedOn    = DateTime.UtcNow,
                        IsObsolete   = false
                    };
                    pnc.FullPNC = $"{pnc.ModelId}-{pnc.EqCode}-{pnc.ModCode}-{pnc.SubAsmCode}-{pnc.DesignOffice}-{pnc.DrawingSeqNo}-{pnc.SeqDigits}-{pnc.MaintLevel}-{pnc.RevSuffix}";

                    var msg = ValidatePNC(pnc);
                    if (msg != null) { errors.Add($"Row {row}: {msg}"); row++; continue; }

                    bool exists = await _db.PartNumberCodes.AnyAsync(p => p.FullPNC == pnc.FullPNC);
                    if (exists) { duplicates.Add(pnc.FullPNC); row++; continue; }

                    _db.PartNumberCodes.Add(pnc);
                    imported.Add(pnc.FullPNC);
                    row++;
                    if (row > 10003) break; // 10000 data rows + 3 header rows
                }

                if (imported.Count > 0) await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error reading file: " + ex.Message });
            }

            return Json(new
            {
                success    = true,
                imported   = imported.Count,
                duplicates = duplicates.Count,
                errors     = errors.Count,
                errorList  = errors,
                dupList    = duplicates,
                message    = $"Imported: {imported.Count}  |  Duplicates skipped: {duplicates.Count}  |  Errors: {errors.Count}"
            });
        }

        // ─── Validation helper ───────────────────────────────────────────────
        private static string? ValidatePNC(PartNumberCode p)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.ModelId,      @"^[A-Z]{2,4}[0-9]?$"))
                return $"ModelId '{p.ModelId}' is invalid (2-4 letters, optional trailing digit).";
            if (string.IsNullOrWhiteSpace(p.EqCode))
                return "Chapter (EqCode) is required.";
            if (string.IsNullOrWhiteSpace(p.ModCode))
                return "Section (ModCode) is required.";
            if (string.IsNullOrWhiteSpace(p.SubAsmCode))
                return "Sub Sec (SubAsmCode) is required.";
            if (string.IsNullOrWhiteSpace(p.DesignOffice))
                return "Design Office is required.";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.DrawingSeqNo, @"^[0-9]{3}$"))
                return $"Drawing Seq. No. '{p.DrawingSeqNo}' is invalid (exactly 3 digits).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.SeqDigits,    @"^[0-9]{5}$"))
                return $"Seq. No. '{p.SeqDigits}' is invalid (exactly 5 digits).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.MaintLevel, @"^[0-9]{2}$"))
                return $"Technical Spec. No. '{p.MaintLevel}' is invalid (exactly 2 digits).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.RevSuffix, @"^[A-Z]$"))
                return $"Colour Scheme '{p.RevSuffix}' is invalid (single letter A-Z).";
            return null;
        }
    }
}
