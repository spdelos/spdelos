using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("LicenseManagement", Schema = "dbo")]
    public partial class LicenseManagement
    {
        [Column("LicenseId")]
        [Key]
        public int LicenseId { get; set; }
        [Column("CompanyId")] 
        public int CompanyId { get; set; }
        [Column("Encrypted")] 
        public string? Encrypted { get; set; }
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")] 
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime UpdatedOn { get; set; }
    }
}
