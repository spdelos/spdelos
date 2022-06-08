using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("CompanyInformation", Schema = "dbo")]
    public partial class CompanyInformation
    {
        [Column("CompanyId")]
        [Key]
        public int CompanyId { get; set; }
        [Column("Name")] 
        public string? Name { get; set; }
        [Column("Location")] 
        public string? Location { get; set; }
        [Column("Year")] 
        public string? Year { get; set; }
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")] 
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime? UpdatedOn { get; set; }
        [Column("SharedPath")] 
        public string? SharedPath { get; set; }
    }
}
