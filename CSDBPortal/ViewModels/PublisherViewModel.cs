using CSDBPortal.Models;

namespace CSDBPortal.ViewModels
{
    public class PublisherViewModel
    {
        public List<Project> Projects { get; set; } = new();

        /// <summary>PNG-only image assets available as OEM logos.</summary>
        public List<ImageAsset> PngLogos { get; set; } = new();
    }
}
