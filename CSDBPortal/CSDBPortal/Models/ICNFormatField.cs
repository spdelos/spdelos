using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("ICNFormatFields", Schema = "dbo")]
    public class ICNFormatField
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("ICNFormatMasterFieldId")] 
        public int ICNFormatMasterFieldId { get; set; }
        [Column("ICNFormatId")] 
        public int ICNFormatId { get; set; }
        [Column("DisplayOrder")] 
        public int DisplayOrder { get; set; }
    }
}
