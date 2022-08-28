using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("UserDetails", Schema = "dbo")]
    public partial class UserDetail
    {
        [Column("UserDetailId")]
        [Key]
        public int UserDetailId { get; set; }
        [Column("UserId")]
        public string? UserId { get; set; }
        [Column("CompanyId")]
        public int CompanyId { get; set; }
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")]
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")]
        public DateTime UpdatedOn { get; set; }
        [Column("DesignationFk")]
        public int? DesignationFk { get; set; }
    }
}
