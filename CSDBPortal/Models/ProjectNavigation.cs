using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ProjectNavigation", Schema = "dbo")]
    public partial class ProjectNavigation
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("ProjectId")] 
        public int ProjectId { get; set; }
        [Column("DMCId")] 
        public int DMCId { get; set; }
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
