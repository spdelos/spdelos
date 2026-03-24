using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("WorkflowInstances", Schema = "dbo")]
    public class WorkflowInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        /// <summary>Set when Step 1 (Project Creation) is completed.</summary>
        public int? ProjectId { get; set; }

        /// <summary>Active | Completed | Cancelled</summary>
        public string Status { get; set; } = "Active";

        /// <summary>1-based index of the next pending step (1–7).</summary>
        public int CurrentStep { get; set; } = 1;

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
