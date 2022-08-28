using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ICNFormatMasterFields", Schema = "dbo")]
    public class ICNFormatMasterField
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Field")] 
        public string? Field { get; set; }
        [Column("Template")] 
        public string? Template { get; set; }
        [Column("ProjectFieldReference")]
        public string? ProjectFieldReference { get; set; }
        [Column("IsForeignKey")]
        public bool? IsForeignKey { get; set; }
        [Column("ReferenceTable")]
        public string? ReferenceTable { get; set; }
        [Column("ReferenceColumn")]
        public string? ReferenceColumn { get; set; }
        [Column("ReferenceKey")]
        public string? ReferenceKey { get; set; }
    }
}
