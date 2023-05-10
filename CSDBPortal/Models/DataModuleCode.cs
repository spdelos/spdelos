using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("DataModuleCode", Schema = "dbo")]
    public class DataModuleCode
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }
        public string? DMC { get; set; }
        public int ProjectId { get; set; }
        public string ModelIdentification { get; set; }
        public string SDC { get; set; }
        public string StandardNumberingSystem { get; set; }
        public string? DC { get; set; }
        public string DCV { get; set; }
        public int InformationCodeId { get; set; }
        public string? ICV { get; set; }
        public int LocationCodeId { get; set; }
        public string TechName { get; set; }
        public string InfoName { get; set; }
	    public int IssueFileId { get; set; }
        public string? xml { get; set; }
        public bool IsBrexXml { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get;set;}
    }
}
