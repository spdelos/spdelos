using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("PartNumberCodes", Schema = "dbo")]
    public class PartNumberCode
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string ModelId { get; set; } = "";       // Seg 1: e.g. KAV, KAV1

        [Required, MaxLength(2)]
        public string EqCode { get; set; } = "";        // Seg 2: 70-80

        [Required, MaxLength(3)]
        public string ModCode { get; set; } = "";       // Seg 3: EGN, FAN…

        [Required, MaxLength(3)]
        public string SubAsmCode { get; set; } = "";    // Seg 4: FCA…

        [Required, MaxLength(5)]
        public string SeqDigits { get; set; } = "";     // Seg 5: 00001

        [Required, MaxLength(1)]
        public string MaintLevel { get; set; } = "";    // Seg 6: O/I/D

        [Required, MaxLength(1)]
        public string RevSuffix { get; set; } = "";     // Seg 7: A-Z

        [Required, MaxLength(50)]
        public string FullPNC { get; set; } = "";       // Assembled PNC (unique)

        public bool IsObsolete { get; set; } = false;

        [MaxLength(256)]
        public string? ObsoletedBy { get; set; }

        public DateTime? ObsoletedOn { get; set; }

        [MaxLength(256)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
