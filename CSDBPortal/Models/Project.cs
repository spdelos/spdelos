using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("Project", Schema = "dbo")]
    public partial class Project
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        [Column("EndItem")]
        public string? EndItem { get; set; }
        [Column("Name")]
        public string? Name { get; set; }
        [Column("Title")]
        public string? Title { get; set; }
        [Column("IssueNoId")]
        public int IssueNoId { get; set; }
        [Column("IcnformatId")]
        public int IcnformatId { get; set; }
        [Column("SnssetId")]
        public int SNSSetId { get; set; }
        [Column("InformationCodeId")]
        public int InformationCodeId { get; set; }
        [Column("LocationCodeId")]
        public int LocationCodeId { get; set; }
        [Column("ModelIdentification")]
        public string? ModelIdentification { get; set; }
        [Column("Sdc")]
        public string? SDC { get; set; }
        [Column("SubjectLength")]
        public int SubjectLength { get; set; }
        [Column("ProjectCode")]
        public string? ProjectCode { get; set; }
        [Column("RPCId")]
        public int RPCId { get; set; }
        [Column("TrackPercentComplete")]
        public bool TrackPercentComplete { get; set; }
        [Column("CreateDefaultBrex")]
        public bool CreateDefaultBrex { get; set; }
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [Column("ModifiedBy")]
        public string? ModifiedBy { get; set; }
        [Column("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }
    }
}
