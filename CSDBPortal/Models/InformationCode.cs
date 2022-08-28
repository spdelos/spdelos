using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("InformationCodes", Schema = "dbo")]
    public partial class InformationCode
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Code")] 
        public string? Code { get; set; }
        [Column("Description")] 
        public string? Description { get; set; }
        [Column("DataModuleId")] 
        public int DataModuleId { get; set; }
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")] 
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime UpdatedOn { get; set; }
        [Column("InformationCodeSetId")] 
        public int InformationCodeSetId { get; set; }
    }
}
