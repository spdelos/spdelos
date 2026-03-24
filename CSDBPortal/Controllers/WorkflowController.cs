using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CSDBPortal.Controllers
{
    public class WorkflowController : BaseController
    {
        // ── Step catalogue ────────────────────────────────────────────────
        public static readonly (int Number, string Name, string ReferenceType)[] StepDefinitions =
        {
            (1, "Project Creation",          "Project"),
            (2, "DMC Creation",              "DMC"),
            (3, "Assignment to Author",      "User"),
            (4, "ICN Creation",              "ICN"),
            (5, "Assignment to Illustrator", "User"),
            (6, "Review after Check In",     ""),
            (7, "Publish",                   ""),
        };

        private readonly ApplicationDbContext _db;

        public WorkflowController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ── INDEX ─────────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var instances = await _db.WorkflowInstances.ToListAsync();
            var projects  = await _db.Projects.OrderBy(p => p.Name).ToListAsync();

            var projectMap = projects.ToDictionary(p => p.Id, p => p.Name);

            // Count completed steps per workflow in one query
            var stepCounts = await _db.WorkflowSteps
                .Where(s => s.CompletedOn != null)
                .GroupBy(s => s.WorkflowInstanceId)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);

            var displays = instances.Select(i => new WorkflowInstanceDisplay
            {
                Id                 = i.Id,
                Title              = i.Title,
                ProjectId          = i.ProjectId,
                ProjectName        = i.ProjectId.HasValue && projectMap.TryGetValue(i.ProjectId.Value, out var pn) ? pn : null,
                Status             = i.Status,
                CurrentStep        = i.CurrentStep,
                CurrentStepName    = i.CurrentStep <= StepDefinitions.Length ? StepDefinitions[i.CurrentStep - 1].Name : "Complete",
                CompletedStepsCount = stepCounts.TryGetValue(i.Id, out var c) ? c : 0,
                CreatedBy          = i.CreatedBy,
                CreatedOn          = i.CreatedOn,
                UpdatedOn          = i.UpdatedOn
            }).OrderByDescending(i => i.CreatedOn).ToList();

            var vm = new WorkflowViewModel
            {
                Workflows       = displays,
                Projects        = projects,
                TotalCount      = instances.Count,
                ActiveCount     = instances.Count(i => i.Status == "Active"),
                CompletedCount  = instances.Count(i => i.Status == "Completed"),
                CancelledCount  = instances.Count(i => i.Status == "Cancelled"),
            };

            return View(vm);
        }

        // ── CREATE WORKFLOW ───────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Json(new { status = false, message = "Title is required." });

            var instance = new WorkflowInstance
            {
                Title     = title.Trim(),
                Status    = "Active",
                CurrentStep = 1,
                CreatedBy = User.Identity!.Name!,
                CreatedOn = DateTime.UtcNow,
            };
            _db.WorkflowInstances.Add(instance);
            await _db.SaveChangesAsync();

            // Seed all 7 step rows
            var steps = StepDefinitions.Select(s => new WorkflowStep
            {
                WorkflowInstanceId = instance.Id,
                StepNumber         = s.Number,
                StepName           = s.Name,
            }).ToList();
            _db.WorkflowSteps.AddRange(steps);
            await _db.SaveChangesAsync();

            return Json(new { status = true, id = instance.Id });
        }

        // ── GET DETAIL (steps) ────────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetDetail(int workflowId)
        {
            var instance = await _db.WorkflowInstances.FirstOrDefaultAsync(i => i.Id == workflowId);
            if (instance == null)
                return Json(new { status = false, message = "Workflow not found." });

            var steps = await _db.WorkflowSteps
                .Where(s => s.WorkflowInstanceId == workflowId)
                .OrderBy(s => s.StepNumber)
                .ToListAsync();

            var stepDisplays = steps.Select(s => new WorkflowStepDisplay
            {
                Id            = s.Id,
                StepNumber    = s.StepNumber,
                StepName      = s.StepName,
                IsCompleted   = s.CompletedOn.HasValue,
                CompletedBy   = s.CompletedBy,
                CompletedOn   = s.CompletedOn,
                Notes         = s.Notes,
                ReferenceId   = s.ReferenceId,
                ReferenceType = s.ReferenceType,
            }).ToList();

            string? projectName = null;
            if (instance.ProjectId.HasValue)
            {
                var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == instance.ProjectId.Value);
                projectName = project?.Name;
            }

            return Json(new
            {
                status      = true,
                id          = instance.Id,
                title       = instance.Title,
                projectName,
                wfStatus    = instance.Status,
                currentStep = instance.CurrentStep,
                createdBy   = instance.CreatedBy,
                createdOn   = instance.CreatedOn.ToLocalTime().ToString("dd MMM yyyy HH:mm"),
                steps       = stepDisplays
            });
        }

        // ── COMPLETE A STEP ───────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> CompleteStep(
            int workflowId,
            int stepNumber,
            string? notes,
            int? referenceId,
            string? referenceType,
            string? referenceLabel)
        {
            var instance = await _db.WorkflowInstances.FirstOrDefaultAsync(i => i.Id == workflowId);
            if (instance == null)
                return Json(new { status = false, message = "Workflow not found." });
            if (instance.Status != "Active")
                return Json(new { status = false, message = "This workflow is not active." });

            var step = await _db.WorkflowSteps
                .FirstOrDefaultAsync(s => s.WorkflowInstanceId == workflowId && s.StepNumber == stepNumber);
            if (step == null)
                return Json(new { status = false, message = "Step not found." });
            if (step.CompletedOn.HasValue)
                return Json(new { status = false, message = "This step is already completed." });

            step.CompletedBy   = User.Identity!.Name;
            step.CompletedOn   = DateTime.UtcNow;
            step.Notes         = notes?.Trim();
            step.ReferenceId   = referenceId;
            step.ReferenceType = referenceType?.Trim();

            // If step 1 completes with a project reference, link it to the workflow
            if (stepNumber == 1 && referenceId.HasValue)
                instance.ProjectId = referenceId;

            // Advance current step pointer
            var nextStep = stepNumber + 1;
            instance.CurrentStep = nextStep <= StepDefinitions.Length ? nextStep : stepNumber;
            instance.UpdatedBy   = User.Identity.Name;
            instance.UpdatedOn   = DateTime.UtcNow;

            // Auto-complete workflow when all 7 steps are done
            var allSteps = await _db.WorkflowSteps
                .Where(s => s.WorkflowInstanceId == workflowId)
                .ToListAsync();
            // Mark the current step as completed in memory before checking
            var allDone = allSteps.All(s => s.StepNumber == stepNumber || s.CompletedOn.HasValue);
            if (allDone)
                instance.Status = "Completed";

            await _db.SaveChangesAsync();

            return Json(new { status = true });
        }

        // ── CANCEL WORKFLOW ───────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> CancelWorkflow(int workflowId)
        {
            var instance = await _db.WorkflowInstances.FirstOrDefaultAsync(i => i.Id == workflowId);
            if (instance == null)
                return Json(new { status = false, message = "Workflow not found." });

            instance.Status    = "Cancelled";
            instance.UpdatedBy = User.Identity!.Name;
            instance.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Json(new { status = true });
        }

        // ── REPORT (JSON) ─────────────────────────────────────────────────
        [HttpGet]
        public async Task<JsonResult> GetReport(
            string? status,
            string? dateFrom,
            string? dateTo,
            int? projectId)
        {
            var query = _db.WorkflowInstances.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
                query = query.Where(i => i.Status == status);

            if (projectId.HasValue)
                query = query.Where(i => i.ProjectId == projectId);

            if (DateTime.TryParse(dateFrom, out var df))
                query = query.Where(i => i.CreatedOn >= df);

            if (DateTime.TryParse(dateTo, out var dt))
                query = query.Where(i => i.CreatedOn <= dt.AddDays(1));

            var instances = await query.OrderByDescending(i => i.CreatedOn).ToListAsync();
            var ids       = instances.Select(i => i.Id).ToList();

            var allSteps = await _db.WorkflowSteps
                .Where(s => ids.Contains(s.WorkflowInstanceId))
                .ToListAsync();

            var projects   = await _db.Projects.ToDictionaryAsync(p => p.Id, p => p.Name);
            var stepsLookup = allSteps.ToLookup(s => s.WorkflowInstanceId);

            var rows = instances.Select(i =>
            {
                var steps = stepsLookup[i.Id].OrderBy(s => s.StepNumber).ToList();
                WorkflowStep? S(int n) => steps.FirstOrDefault(s => s.StepNumber == n);
                return new WorkflowReportRow
                {
                    Id          = i.Id,
                    Title       = i.Title,
                    ProjectName = i.ProjectId.HasValue && projects.TryGetValue(i.ProjectId.Value, out var pn) ? pn : null,
                    Status      = i.Status,
                    CreatedBy   = i.CreatedBy,
                    CreatedOn   = i.CreatedOn.ToLocalTime(),
                    Step1On     = S(1)?.CompletedOn?.ToLocalTime(), Step1By = S(1)?.CompletedBy,
                    Step2On     = S(2)?.CompletedOn?.ToLocalTime(), Step2By = S(2)?.CompletedBy,
                    Step3On     = S(3)?.CompletedOn?.ToLocalTime(), Step3By = S(3)?.CompletedBy,
                    Step4On     = S(4)?.CompletedOn?.ToLocalTime(), Step4By = S(4)?.CompletedBy,
                    Step5On     = S(5)?.CompletedOn?.ToLocalTime(), Step5By = S(5)?.CompletedBy,
                    Step6On     = S(6)?.CompletedOn?.ToLocalTime(), Step6By = S(6)?.CompletedBy,
                    Step7On     = S(7)?.CompletedOn?.ToLocalTime(), Step7By = S(7)?.CompletedBy,
                };
            }).ToList();

            return Json(new { status = true, data = rows });
        }

        // ── EXPORT CSV ────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> ExportCsv(
            string? status,
            string? dateFrom,
            string? dateTo,
            int? projectId)
        {
            var reportResult = await GetReport(status, dateFrom, dateTo, projectId);
            var payload = reportResult.Value as dynamic;
            var rows = payload?.data as IEnumerable<WorkflowReportRow> ?? Enumerable.Empty<WorkflowReportRow>();

            var sb = new StringBuilder();
            sb.AppendLine("Id,Title,Project,Status,Created By,Created On," +
                          "Step 1 Date,Step 1 By," +
                          "Step 2 Date,Step 2 By," +
                          "Step 3 Date,Step 3 By," +
                          "Step 4 Date,Step 4 By," +
                          "Step 5 Date,Step 5 By," +
                          "Step 6 Date,Step 6 By," +
                          "Step 7 Date,Step 7 By");

            static string Fmt(DateTime? d) => d.HasValue ? d.Value.ToString("dd MMM yyyy HH:mm") : "";
            static string Csv(string? v) => $"\"{(v ?? "").Replace("\"", "\"\"")}\"";

            foreach (var r in rows)
            {
                sb.AppendLine(string.Join(",",
                    r.Id, Csv(r.Title), Csv(r.ProjectName), Csv(r.Status),
                    Csv(r.CreatedBy), Fmt(r.CreatedOn),
                    Fmt(r.Step1On), Csv(r.Step1By),
                    Fmt(r.Step2On), Csv(r.Step2By),
                    Fmt(r.Step3On), Csv(r.Step3By),
                    Fmt(r.Step4On), Csv(r.Step4By),
                    Fmt(r.Step5On), Csv(r.Step5By),
                    Fmt(r.Step6On), Csv(r.Step6By),
                    Fmt(r.Step7On), Csv(r.Step7By)));
            }

            var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
            return File(bytes, "text/csv", $"workflow-report-{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}
