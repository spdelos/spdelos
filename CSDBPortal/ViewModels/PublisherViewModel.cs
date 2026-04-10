using CSDBPortal.Models;

namespace CSDBPortal.ViewModels
{
    public class PublisherViewModel
    {
        public List<Project> Projects { get; set; } = new();

        /// <summary>PNG-only image assets available as OEM logos.</summary>
        public List<ImageAsset> PngLogos { get; set; } = new();

        /// <summary>Portal-wide provider key stored in ApplicationSettings. Null if not yet generated.</summary>
        public string? ProviderKey { get; set; }

        /// <summary>All published .nav package license records.</summary>
        public List<IetpLicense> IetpLicenses { get; set; } = new();
    }
}
