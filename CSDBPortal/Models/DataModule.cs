using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("DataModule", Schema = "dbo")]
    public partial class DataModule
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int StandardNumberingSystemId { get; set; }
        public int InformationCodeId { get; set; }
        public string? InformationCodeDesc { get; set; }
        public string? SortInfoCodeBy { get; set; }
        public string Title { get; set; } = null!;
        public string? Dmc { get; set; }
        public int DataModuleTypeId { get; set; }
        public string DisassemblyCodeVariant { get; set; } = null!;
        public int ItemLocationId { get; set; }
        public string? AssocateEagleTalk { get; set; }
        public string? Lcn { get; set; }
        [Column("ALC")]
        public string? Alc { get; set; }
        public string? Lcntype { get; set; }
        public string? TaskId { get; set; }
        public string? RefNo { get; set; }
        public string? Cage { get; set; }
        public int? Status { get; set; }
        public int IssueNoId { get; set; }
        public string? InWork { get; set; }
        public string? Code { get; set; }
        public string? Ent { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? AssignedTo { get; set; }
        public string? Path { get; set; }
    }
}
