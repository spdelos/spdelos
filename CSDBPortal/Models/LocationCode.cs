using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("LocationCodes", Schema = "dbo")]
    public partial class LocationCode
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Code")] 
        public string? Code { get; set; }
        [Column("Description")] 
        public string? Description { get; set; }
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        [Column("CreatedOn")] 
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedBy")] 
        public string? UpdatedBy { get; set; }
        [Column("UpdatedOn")] 
        public DateTime UpdatedOn { get; set; }
        [Column("LocationCodeSetId")] 
        public int LocationCodeSetId { get; set; }
    }
}
