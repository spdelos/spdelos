using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    /// <summary>
    /// Maps to the IETP_License table used by the NavIETM Viewer.
    /// A record is written here each time a .nav file is published so the Viewer
    /// knows whether the package is secured and can associate a LicenseKey later.
    /// </summary>
    [Table("IETP_License", Schema = "dbo")]
    public class IetpLicense
    {
        [Key]
        public int Id { get; set; }

        /// <summary>The .nav filename (e.g. PMC_F100.nav).</summary>
        [MaxLength(500)]
        public string IETP { get; set; } = string.Empty;

        /// <summary>True when the package was published as Secured.</summary>
        public bool IsSecured { get; set; }

        /// <summary>
        /// The provider/client key embedded in License.lic (UTF-8 bytes).
        /// Null for Unsecured packages.
        /// </summary>
        public byte[]? ClientKey { get; set; }

        /// <summary>
        /// Set to NULL at publish time; filled in by the Viewer admin after activation.
        /// </summary>
        [MaxLength(50)]
        public string? LicenseKey { get; set; }

        /// <summary>True when the package was published as Draft (trial watermarks).</summary>
        public bool IsTrial { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
