using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("LocationCodeSets", Schema = "dbo")]
    public class LocationCodeSet
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
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
    }
}
