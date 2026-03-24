using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("Stylesheets", Schema = "dbo")]
    public class Stylesheet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? Remarks { get; set; }

        public string UploadedBy { get; set; } = string.Empty;

        public DateTime UploadedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
