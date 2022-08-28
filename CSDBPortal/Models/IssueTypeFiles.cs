using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("IssueTypeFiles", Schema = "dbo")]
    public class IssueTypeFile
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("IssueNoId")] 
        public int IssueNoId { get; set; }
        [Column("Name")] 
        public string Name { get; set; }
        [Column("Data")] 
        public string Data { get; set; }
        [Column("CreateOn")] 
        public DateTime CreateOn { get; set; }
        [Column("CreatedBy")] 
        public string CreatedBy { get; set; }
        [Column("IssueNo")] 
        public IssueNo IssueNo { get; set; }
    }
}
