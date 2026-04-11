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

        // ── Package code registry ─────────────────────────────────────────────
        /// <summary>Registered PMC codes not yet consumed by a publish.</summary>
        public List<PackageCode> AvailablePmcCodes { get; set; } = new();

        /// <summary>Registered DDN codes not yet consumed by a publish.</summary>
        public List<PackageCode> AvailableDdnCodes { get; set; } = new();

        /// <summary>Full registry of all codes (used and unused) — shown in License tab.</summary>
        public List<PackageCode> AllPackageCodes { get; set; } = new();

        /// <summary>
        /// For published codes: whether the archived .nav file still exists on the server.
        /// Keyed by PackageCode.Id.
        /// </summary>
        public Dictionary<int, bool> PackageFileStatus { get; set; } = new();

        // ── Permission flags (set from the signed-in user's claims) ──────────
        public bool CanPublishIetp    { get; set; }
        public bool CanExportPdf      { get; set; }
        public bool CanManageLicense  { get; set; }
    }
}
