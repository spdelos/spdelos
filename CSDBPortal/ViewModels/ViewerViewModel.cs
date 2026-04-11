using CSDBPortal.Models;

namespace CSDBPortal.ViewModels
{
    public class ViewerViewModel
    {
        public List<Project> Projects { get; set; } = new();
        public bool CanView { get; set; }
    }
}
