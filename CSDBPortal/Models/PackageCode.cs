using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    /// <summary>
    /// Tracks registered PMC/DDN package codes.
    /// Once created a record is never deleted — it may only transition
    /// from unused (IsPublished = false) to published (IsPublished = true).
    /// </summary>
    [Table("PackageCodes", Schema = "dbo")]
    public class PackageCode
    {
        [Key]
        public int Id { get; set; }

        /// <summary>The assembled S1000D code, e.g. PMC-AAA-BBBBB-12345-01</summary>
        [Required, MaxLength(200)]
        public string Code { get; set; } = null!;

        /// <summary>"PMC" or "DDN"</summary>
        [Required, MaxLength(3)]
        public string CodeType { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(256)]
        public string CreatedBy { get; set; } = null!;

        /// <summary>False until used in a successful publish.</summary>
        public bool IsPublished { get; set; } = false;

        public DateTime? PublishedAt { get; set; }

        [MaxLength(256)]
        public string? PublishedBy { get; set; }

        /// <summary>The .nav filename this code was published as.</summary>
        [MaxLength(500)]
        public string? PackageFilename { get; set; }

        public int? ProjectId { get; set; }

        [MaxLength(200)]
        public string? ProjectName { get; set; }
    }
}
