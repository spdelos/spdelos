using CSDBPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace CSDBPortal.ViewModels
{
    public class AdministrationViewModel
    {
        public List<IdentityUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<string> Features { get; set; }
        public int ProjectCompleted { get; set; }
        public int UserCount { get; set; }
        public int ActiveUsers { get; set; }
        public List<QuickAccessItem> QuickAccessItems { get; set; } = new();
    }
}
