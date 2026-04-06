using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("PNCLookups", Schema = "dbo")]
    public class PNCLookup
    {
        [Key]
        public int Id { get; set; }

        /// <summary>EqCode | ModCode | SubAsm | MaintLevel</summary>
        [Required, MaxLength(20)]
        public string LookupType { get; set; } = "";

        [Required, MaxLength(10)]
        public string Code { get; set; } = "";

        [Required, MaxLength(200)]
        public string Description { get; set; } = "";

        public int SortOrder { get; set; }

        [MaxLength(256)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
