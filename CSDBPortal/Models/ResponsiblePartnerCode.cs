using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ResponsiblePartnerCodes", Schema = "dbo")]
    public partial class ResponsiblePartnerCode
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Code")] 
        public string? Code { get; set; }
        [Column("Rpccage")] 
        public string? Rpccage { get; set; }
        [Column("OrginatorCage")] 
        public string? OrginatorCage { get; set; }
    }
}
