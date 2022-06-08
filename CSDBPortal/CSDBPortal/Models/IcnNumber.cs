using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("IcnNumbers", Schema = "dbo")]
    public partial class IcnNumber
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Number")] 
        public string? Number { get; set; }
        [Column("DataModuleId")] 
        public int DataModuleId { get; set; }
        [Column("ProjectId")] 
        public int ProjectId { get; set; }
        [Column("ImagePath")] 
        public string? ImagePath { get; set; }
        [Column("IsAllocated")] 
        public bool IsAllocated { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime UpdatedOn { get; set; }
        [Column("ICNFormatId")]
        public int ICNFormatId { get; set; }
    }
}
