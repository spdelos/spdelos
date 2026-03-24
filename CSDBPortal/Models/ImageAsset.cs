using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ImageAssets", Schema = "dbo")]
    public class ImageAsset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string MimeType { get; set; } = string.Empty;

        /// <summary>Base64-encoded file content.</summary>
        public string? Data { get; set; }

        public string? Remarks { get; set; }

        public string UploadedBy { get; set; } = string.Empty;

        public DateTime UploadedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
