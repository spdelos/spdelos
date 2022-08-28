using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("Designations", Schema = "dbo")]
    public partial class Designation
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("Name")] 
        public string? Name { get; set; }
    }
}
