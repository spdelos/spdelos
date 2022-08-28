using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("AspNetRoles", Schema = "dbo")]
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            Users = new HashSet<AspNetUser>();
        }
        [Column("Id")]
        [Key]
        public string Id { get; set; } = null!;
        [Column("Name")]
        public string? Name { get; set; }
        [Column("NormalizedName")]
        public string? NormalizedName { get; set; }
        [Column("ConcurrencyStamp")]
        public string? ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
