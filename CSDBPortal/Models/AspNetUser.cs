using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("AspNetUsers", Schema = "dbo")]
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Roles = new HashSet<AspNetRole>();
        }

        [Column("Id")]
        [Key]
        public string Id { get; set; } = null!;
        [Column("UserName")] 
        public string? UserName { get; set; }
        [Column("NormalizedUserName")] 
        public string? NormalizedUserName { get; set; }
        [Column("Email")] 
        public string? Email { get; set; }
        [Column("NormalizedEmail")] 
        public string? NormalizedEmail { get; set; }
        [Column("EmailConfirmed")] 
        public bool EmailConfirmed { get; set; }
        [Column("PasswordHash")] 
        public string? PasswordHash { get; set; }
        [Column("SecurityStamp")] 
        public string? SecurityStamp { get; set; }
        [Column("ConcurrencyStamp")] 
        public string? ConcurrencyStamp { get; set; }
        [Column("PhoneNumber")] 
        public string? PhoneNumber { get; set; }
        [Column("PhoneNumberConfirmed")] 
        public bool PhoneNumberConfirmed { get; set; }
        [Column("TwoFactorEnabled")] 
        public bool TwoFactorEnabled { get; set; }
        [Column("LockoutEnd")] 
        public DateTimeOffset? LockoutEnd { get; set; }
        [Column("LockoutEnabled")] 
        public bool LockoutEnabled { get; set; }
        [Column("AccessFailedCount")] 
        public int AccessFailedCount { get; set; }
        [Column("Discriminator")] 
        public string Discriminator { get; set; } = null!;

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
