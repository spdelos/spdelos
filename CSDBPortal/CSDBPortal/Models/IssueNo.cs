using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("IssueNo", Schema = "dbo")]
    public partial class IssueNo
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Name")] 
        public string? Name { get; set; }
        [Column("Path")] 
        public string? Path { get; set; }
        [Column("IsDelete")] 
        public bool IsDelete { get; set; }
        [Column("CreateOn")] 
        public DateTime CreateOn { get; set; }
        [Column("CreatedBy")] 
        public string? CreatedBy { get; set; }
        
        public List<IssueTypeFile> IssueTypeFiles { get; set; }
    }
}
