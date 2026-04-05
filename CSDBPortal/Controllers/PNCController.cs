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

        // ─── GET: next available SeqDigits for a model ───────────────────────
        [HttpGet]
        public async Task<JsonResult> GetNextSeqDigits(string modelId)
        {
            if (string.IsNullOrWhiteSpace(modelId))
                return Json(new { success = false });
            try
            {
                var max = await _db.PartNumberCodes
                    .Where(p => p.ModelId == modelId.Trim().ToUpper())
                    .MaxAsync(p => (string?)p.SeqDigits);

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
            string? seg5, string? seg6, string? seg7, bool includeObsolete = true)
        {
            try
            {
                var q = _db.PartNumberCodes.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(seg1)) q = q.Where(p => p.ModelId    == seg1.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg2)) q = q.Where(p => p.EqCode     == seg2.Trim());
                if (!string.IsNullOrWhiteSpace(seg3)) q = q.Where(p => p.ModCode    == seg3.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg4)) q = q.Where(p => p.SubAsmCode == seg4.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg5)) q = q.Where(p => p.SeqDigits  == seg5.Trim());
                if (!string.IsNullOrWhiteSpace(seg6)) q = q.Where(p => p.MaintLevel == seg6.Trim().ToUpper());
                if (!string.IsNullOrWhiteSpace(seg7)) q = q.Where(p => p.RevSuffix  == seg7.Trim().ToUpper());
                if (!includeObsolete)                 q = q.Where(p => !p.IsObsolete);

                var rows = await q.OrderBy(p => p.FullPNC)
                                  .Select(p => new {
                                      p.Id, p.ModelId, p.EqCode, p.ModCode, p.SubAsmCode,
                                      p.SeqDigits, p.MaintLevel, p.RevSuffix, p.FullPNC,
                                      p.IsObsolete, p.ObsoletedBy,
                                      ObsoletedOn = p.ObsoletedOn.HasValue
                                          ? p.ObsoletedOn.Value.ToString("yyyy-MM-dd HH:mm") : null,
                                      p.CreatedBy,
                                      CreatedOn = p.CreatedOn.ToString("yyyy-MM-dd HH:mm")
                                  })
                                  .ToListAsync();

                return Json(new { success = true, data = rows });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message, data = Array.Empty<object>() });
            }
        }

        // ─── POST: save single PNC ───────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> SavePNC([FromBody] PartNumberCode pnc)
        {
            if (pnc == null)
                return Json(new { success = false, message = "No data received." });

            pnc.ModelId    = (pnc.ModelId    ?? "").Trim().ToUpper();
            pnc.EqCode     = (pnc.EqCode     ?? "").Trim();
            pnc.ModCode    = (pnc.ModCode    ?? "").Trim().ToUpper();
            pnc.SubAsmCode = (pnc.SubAsmCode ?? "").Trim().ToUpper();
            pnc.SeqDigits  = (pnc.SeqDigits  ?? "").Trim();
            pnc.MaintLevel = (pnc.MaintLevel ?? "").Trim().ToUpper();
            pnc.RevSuffix  = (pnc.RevSuffix  ?? "").Trim().ToUpper();
            pnc.FullPNC    = $"{pnc.ModelId}-{pnc.EqCode}-{pnc.ModCode}-{pnc.SubAsmCode}-{pnc.SeqDigits}-{pnc.MaintLevel}-{pnc.RevSuffix}";
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
                if (pnc.IsObsolete) return Json(new { success = false, message = "Already obsolete." });

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
            string? seg5, string? seg6, string? seg7, bool includeObsolete = true)
        {
            var q = _db.PartNumberCodes.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(seg1)) q = q.Where(p => p.ModelId    == seg1.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg2)) q = q.Where(p => p.EqCode     == seg2.Trim());
            if (!string.IsNullOrWhiteSpace(seg3)) q = q.Where(p => p.ModCode    == seg3.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg4)) q = q.Where(p => p.SubAsmCode == seg4.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg5)) q = q.Where(p => p.SeqDigits  == seg5.Trim());
            if (!string.IsNullOrWhiteSpace(seg6)) q = q.Where(p => p.MaintLevel == seg6.Trim().ToUpper());
            if (!string.IsNullOrWhiteSpace(seg7)) q = q.Where(p => p.RevSuffix  == seg7.Trim().ToUpper());
            if (!includeObsolete)                 q = q.Where(p => !p.IsObsolete);

            var rows = await q.OrderBy(p => p.FullPNC).ToListAsync();

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Part Numbers");

            // Header
            string[] headers = { "ModelId", "EqCode", "ModCode", "SubAsmCode",
                                  "SeqDigits", "MaintLevel", "RevSuffix", "Full PNC",
                                  "Status", "Created By", "Created On",
                                  "Obsoleted By", "Obsoleted On" };
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
                ws.Cell(r, 5).Value  = p.SeqDigits;
                ws.Cell(r, 6).Value  = p.MaintLevel;
                ws.Cell(r, 7).Value  = p.RevSuffix;
                ws.Cell(r, 8).Value  = p.FullPNC;
                ws.Cell(r, 9).Value  = p.IsObsolete ? "Obsolete" : "Active";
                ws.Cell(r, 10).Value = p.CreatedBy   ?? "";
                ws.Cell(r, 11).Value = p.CreatedOn.ToString("yyyy-MM-dd HH:mm");
                ws.Cell(r, 12).Value = p.ObsoletedBy ?? "";
                ws.Cell(r, 13).Value = p.ObsoletedOn.HasValue
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
            var ws = wb.Worksheets.Add("PNC_Template");

            string[] headers = { "ModelId (Seg1)", "EqCode (Seg2)", "ModCode (Seg3)",
                                  "SubAsmCode (Seg4)", "SeqDigits (Seg5)", "MaintLevel (Seg6)", "RevSuffix (Seg7)" };
            for (int c = 0; c < headers.Length; c++)
            {
                ws.Cell(1, c + 1).Value = headers[c];
                ws.Cell(1, c + 1).Style.Font.Bold = true;
                ws.Cell(1, c + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
            }

            ws.Cell(2, 1).Value = "e.g. KAV";
            ws.Cell(2, 2).Value = "e.g. 72";
            ws.Cell(2, 3).Value = "e.g. FAN";
            ws.Cell(2, 4).Value = "e.g. FCA";
            ws.Cell(2, 5).Value = "e.g. 00001";
            ws.Cell(2, 6).Value = "O/I/D";
            ws.Cell(2, 7).Value = "e.g. A";
            for (int c = 1; c <= 7; c++)
                ws.Cell(2, c).Style.Font.Italic = true;

            var wsEq = wb.Worksheets.Add("Equipment Codes");
            wsEq.Cell(1, 1).Value = "Code"; wsEq.Cell(1, 2).Value = "Description";
            wsEq.Cell(1, 1).Style.Font.Bold = true; wsEq.Cell(1, 2).Style.Font.Bold = true;
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

            var wsMod = wb.Worksheets.Add("Module Codes");
            wsMod.Cell(1, 1).Value = "Code"; wsMod.Cell(1, 2).Value = "Description";
            wsMod.Cell(1, 1).Style.Font.Bold = true; wsMod.Cell(1, 2).Style.Font.Bold = true;
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

            ws.Columns().AdjustToContents();
            wsEq.Columns().AdjustToContents();
            wsMod.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PNC_Template.xlsx");
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
                string[] expectedHeaders = { "ModelId", "EqCode", "ModCode", "SubAsmCode", "SeqDigits", "MaintLevel", "RevSuffix" };
                for (int c = 0; c < expectedHeaders.Length; c++)
                {
                    var h = ws.Cell(1, c + 1).GetString().Trim();
                    if (!h.StartsWith(expectedHeaders[c], StringComparison.OrdinalIgnoreCase))
                        return Json(new { success = false, message = $"Incompatible format — column {c+1} expected '{expectedHeaders[c]}', found '{h}'. Use the PNC template." });
                }

                int row = 2;
                while (true)
                {
                    var cell = ws.Cell(row, 1).GetString().Trim();
                    if (string.IsNullOrEmpty(cell) && row > 2) break;
                    if (string.IsNullOrEmpty(cell)) { row++; continue; }

                    var pnc = new PartNumberCode
                    {
                        ModelId    = ws.Cell(row, 1).GetString().Trim().ToUpper(),
                        EqCode     = ws.Cell(row, 2).GetString().Trim(),
                        ModCode    = ws.Cell(row, 3).GetString().Trim().ToUpper(),
                        SubAsmCode = ws.Cell(row, 4).GetString().Trim().ToUpper(),
                        SeqDigits  = ws.Cell(row, 5).GetString().Trim(),
                        MaintLevel = ws.Cell(row, 6).GetString().Trim().ToUpper(),
                        RevSuffix  = ws.Cell(row, 7).GetString().Trim().ToUpper(),
                        CreatedBy  = User.Identity?.Name,
                        CreatedOn  = DateTime.UtcNow,
                        IsObsolete = false
                    };
                    pnc.FullPNC = $"{pnc.ModelId}-{pnc.EqCode}-{pnc.ModCode}-{pnc.SubAsmCode}-{pnc.SeqDigits}-{pnc.MaintLevel}-{pnc.RevSuffix}";

                    var msg = ValidatePNC(pnc);
                    if (msg != null) { errors.Add($"Row {row}: {msg}"); row++; continue; }

                    bool exists = await _db.PartNumberCodes.AnyAsync(p => p.FullPNC == pnc.FullPNC);
                    if (exists) { duplicates.Add(pnc.FullPNC); row++; continue; }

                    _db.PartNumberCodes.Add(pnc);
                    imported.Add(pnc.FullPNC);
                    row++;
                    if (row > 10000) break;
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.ModelId,    @"^[A-Z]{2,4}[0-9]?$"))
                return $"ModelId '{p.ModelId}' is invalid (2-4 letters, optional trailing digit).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.EqCode,     @"^(70|71|72|73|74|75|76|77|78|79|80)$"))
                return $"EqCode '{p.EqCode}' is invalid (must be 70-80).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.ModCode,    @"^(EGN|EEX|LPC|COU|FAN|ICA|HPC|DCO|TNZ|HPT|LPT|TEC|MGB|AGB)$"))
                return $"ModCode '{p.ModCode}' is not a recognised module code.";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.SubAsmCode, @"^[A-Z]{3}$"))
                return $"SubAsmCode '{p.SubAsmCode}' is invalid (exactly 3 letters).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.SeqDigits,  @"^[0-9]{5}$"))
                return $"SeqDigits '{p.SeqDigits}' is invalid (exactly 5 digits).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.MaintLevel, @"^[OID]$"))
                return $"MaintLevel '{p.MaintLevel}' is invalid (O, I or D).";
            if (!System.Text.RegularExpressions.Regex.IsMatch(p.RevSuffix,  @"^[A-Z]$"))
                return $"RevSuffix '{p.RevSuffix}' is invalid (single letter A-Z).";
            return null;
        }
    }
}
