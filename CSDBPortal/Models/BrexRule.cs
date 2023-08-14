using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("BrexRules", Schema = "dbo")]
    public partial class BrexRule
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        public string? Group { get; set; }
        public string? RuleName { get; set; }
        public string? Xml { get; set; }
        public string? Dmtype { get; set; }
        public int IssueNoId { get; set; }
        public string? XmlTag { get; set; }
        public string? SubXmlTag { get; set; }
        public string? Type { get; set; }
        public int Length { get; set; }
        public string? RangeValue { get; set; }
        public string? MatchValue { get; set; }
        public string? AttributeName { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int ProjectId { get; set; }
    }
}
