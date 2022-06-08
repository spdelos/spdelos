using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("XmlValidation", Schema = "dbo")]
    public partial class XmlValidation
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("DataModuleId")]
        public int DataModuleId { get; set; }
        [Column("UploadStatus")]
        public bool UploadStatus { get; set; }
        [Column("Message")]
        public string? Message { get; set; }
        [Column("UploadedBy")]
        public string? UploadedBy { get; set; }
        [Column("UploadedOn")]
        public DateTime UploadedOn { get; set; }
    }
}
