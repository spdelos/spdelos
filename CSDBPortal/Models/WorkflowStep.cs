using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("WorkflowSteps", Schema = "dbo")]
    public class WorkflowStep
    {
        [Key]
        public int Id { get; set; }

        public int WorkflowInstanceId { get; set; }

        /// <summary>1 = Project Creation … 7 = Publish</summary>
        public int StepNumber { get; set; }

        public string StepName { get; set; } = string.Empty;

        public string? CompletedBy { get; set; }
        public DateTime? CompletedOn { get; set; }
        public string? Notes { get; set; }

        /// <summary>Optional FK to the entity created/used in this step (ProjectId, DmcId, IcnId).</summary>
        public int? ReferenceId { get; set; }

        /// <summary>"Project" | "DMC" | "ICN" | "User" — describes what ReferenceId points to.</summary>
        public string? ReferenceType { get; set; }
    }
}
