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
        public string Field { get; set; }
        [Column("Template")] 
        public string Template { get; set; }
    }
}
