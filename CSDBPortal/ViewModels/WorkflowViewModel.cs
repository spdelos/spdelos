using CSDBPortal.Models;

namespace CSDBPortal.ViewModels
{
    /// <summary>Data passed to the Workflow Index view on first render.</summary>
    public class WorkflowViewModel
    {
        public List<WorkflowInstanceDisplay> Workflows { get; set; } = new();
        public List<Project> Projects { get; set; } = new();
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
    }

    /// <summary>Workflow row in the list / report table.</summary>
    public class WorkflowInstanceDisplay
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CurrentStep { get; set; }
        public string CurrentStepName { get; set; } = string.Empty;
        public int CompletedStepsCount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    /// <summary>Single step row returned to the detail modal.</summary>
    public class WorkflowStepDisplay
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string StepName { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string? CompletedBy { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string? Notes { get; set; }
        public int? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
    }

    /// <summary>Row returned by the report endpoint.</summary>
    public class WorkflowReportRow
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ProjectName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        // Per-step completion timestamps (null = not yet done)
        public DateTime? Step1On { get; set; }
        public string? Step1By { get; set; }
        public DateTime? Step2On { get; set; }
        public string? Step2By { get; set; }
        public DateTime? Step3On { get; set; }
        public string? Step3By { get; set; }
        public DateTime? Step4On { get; set; }
        public string? Step4By { get; set; }
        public DateTime? Step5On { get; set; }
        public string? Step5By { get; set; }
        public DateTime? Step6On { get; set; }
        public string? Step6By { get; set; }
        public DateTime? Step7On { get; set; }
        public string? Step7By { get; set; }
    }
}
