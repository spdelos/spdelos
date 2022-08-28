using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("DataModuleTypes", Schema = "dbo")]
    public partial class DataModuleType
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Name")] 
        public string Name { get; set; } = null!;
        [Column("FileName")] 
        public string FileName { get; set; } = null!;
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")] 
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime UpdatedOn { get; set; }
        [Column("IssueNo")] 
        public int IssueNo { get; set; }
    }
}
