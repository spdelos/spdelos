using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("AspNetUserClaims", Schema = "dbo")]
    public partial class AspNetUserClaim
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("UserId")]
        public string UserId { get; set; } = null!;
        [Column("ClaimType")] 
        public string? ClaimType { get; set; }
        [Column("ClaimValue")] 
        public string? ClaimValue { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
    }
}
