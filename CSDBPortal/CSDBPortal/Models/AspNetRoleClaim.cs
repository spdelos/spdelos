using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("AspNetRoleClaims", Schema = "dbo")]
    public partial class AspNetRoleClaim
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("RoleId")]
        public string RoleId { get; set; } = null!;
        [Column("ClaimType")]
        public string? ClaimType { get; set; }
        [Column("ClaimValue")]
        public string? ClaimValue { get; set; }

        public virtual AspNetRole Role { get; set; } = null!;
    }
}
