using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("DataModuleStatus", Schema = "dbo")]
    public partial class DataModuleStatus
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("DataModuleId")] 
        public int DataModuleId { get; set; }
        [Column("UploadedFile")] 
        public string? UploadedFile { get; set; }
        [Column("Status")] 
        public string? Status { get; set; }
        [Column("UploadedBy")] 
        public string? UploadedBy { get; set; }
        [Column("UploadedOn")] 
        public DateTime UploadedOn { get; set; }
        [Column("IsCurrentVersion")] 
        public bool IsCurrentVersion { get; set; }
    }
}
