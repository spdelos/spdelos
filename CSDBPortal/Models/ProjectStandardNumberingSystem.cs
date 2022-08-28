using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ProjectStandardNumberingSystems", Schema = "dbo")]
    public partial class ProjectStandardNumberingSystem
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("ProjectId")] 
        public int ProjectId { get; set; }
        [Column("Code")] 
        public string? Code { get; set; }
        [Column("Description")] 
        public string? Description { get; set; }
        [Column("Snsid")] 
        public int? Snsid { get; set; }
        [Column("ParentId")] 
        public int ParentId { get; set; }
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
