namespace CSDBPortal
{
    /// <summary>
    /// All permission constants used throughout the application.
    ///
    /// NAMING CONVENTION — every constant must follow this pattern:
    ///
    ///   Top-level module  →  "Module Name"
    ///   Sub-feature       →  "Module Name: Feature"
    ///
    /// This groups related permissions together in the role editor (which sorts
    /// alphabetically) and makes it immediately obvious which module a permission
    /// belongs to.
    ///
    /// HOW TO ADD A NEW PERMISSION
    /// 1. Add a public static field here following the naming convention above.
    /// 2. Apply the check in the controller action:
    ///       if (!HasPermission(Features.YourNewFeature)) return Forbid();
    /// 3. Apply the check in the view:
    ///       @if (User.HasClaim("Permission", Features.YourNewFeature)) { ... }
    /// 4. That's it — the role editor picks up the new constant automatically
    ///    via reflection (see AdministrationManager.GetAdministrationDetailInfoAsync).
    /// </summary>
    public static class Features
    {
        // ── Configuration module ─────────────────────────────────────────────
        public static string IssuAdministration             = "Configuration: Issue Administration";
        public static string InfoCodeSetAdministration      = "Configuration: Info Code Set Administration";
        public static string InfoCodeAdministration         = "Configuration: Info Code Administration";
        public static string IcnFormatAdministration        = "Configuration: ICN Format Administration";
        public static string LocationCodeSetAdministration  = "Configuration: Location Code Set Administration";
        public static string LocationCodeAdministration     = "Configuration: Location Code Administration";
        public static string RPCAdministration              = "Configuration: RPC Administration";
        public static string DataModuleTypeAdministration   = "Configuration: Data Module Type Administration";

        // ── Manage module ────────────────────────────────────────────────────
        public static string SNSAdministration              = "Manage: SNS Administration";
        public static string ProjectAdministration          = "Manage: Project Administration";
        public static string DMCAdministration              = "Manage: DMC Administration";
        public static string AllocationAdministration       = "Manage: Allocation Administration";
        public static string StylesheetAdministration       = "Manage: Stylesheet Administration";
        public static string ImagesAdministration           = "Manage: Images Administration";

        // ── ICN module ───────────────────────────────────────────────────────
        public static string GenerateICNNumber              = "ICN: Generate";
        public static string ViewICNNumber                  = "ICN: View";

        // ── Publisher module ─────────────────────────────────────────────────
        public static string PublisherIetp                  = "Publisher: IETP";
        public static string PublisherPdf                   = "Publisher: PDF";
        public static string PublisherLicense               = "Publisher: License";

        // ── Viewer module ────────────────────────────────────────────────────
        public static string Viewer                         = "Viewer";

        // ── Licensing module ─────────────────────────────────────────────────
        public static string Licensing                      = "Licensing";

        // ── Administration (restricted — admin-only) ─────────────────────────
        public static string UserAndRoleAdministration      = "Administration: User and Role Administration";
    }
}
