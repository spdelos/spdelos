using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSDBPortal.Models
{
    [Table("QuickAccessItems", Schema = "dbo")]
    public class QuickAccessItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Href { get; set; } = string.Empty;
        /// <summary>Font Awesome class string, e.g. "fa-solid fa-hashtag"</summary>
        public string IconClass { get; set; } = string.Empty;
        /// <summary>CSS colour value, e.g. "#2563eb"</summary>
        public string IconColor { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
