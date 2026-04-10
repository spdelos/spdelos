namespace CSDBPortal
{
    /// <summary>
    /// All permission constants used throughout the application.
    ///
    /// IMPORTANT — string VALUES must never be changed once a permission has been deployed,
    /// because those strings are stored verbatim as claims in the database. Changing a value
    /// breaks every role that already holds that permission.
    ///
    /// NAMING CONVENTION for NEW permissions:
    ///   Top-level module  →  "Module Name"
    ///   Sub-feature       →  "Module Name: Feature"
    ///
    /// HOW TO ADD A NEW PERMISSION
    /// 1. Add a public static field here following the convention above.
    /// 2. Guard the controller action:  if (!HasPermission(Features.X)) return PermissionDenied();
    /// 3. Guard the view element:       @if (User.HasClaim("Permission", Features.X)) { ... }
    /// The role editor picks up the new constant automatically via reflection.
    /// </summary>
    public static class Features
    {
        // ── Configuration module ─────────────────────────────────────────────
        public static string IssuAdministration             = "Issue Administration";
        public static string InfoCodeSetAdministration      = "Info Code Set Administration";
        public static string InfoCodeAdministration         = "Info Code Administration";
        public static string IcnFormatAdministration        = "ICN Format Administration";
        public static string LocationCodeSetAdministration  = "Location Code Set Administration";
        public static string LocationCodeAdministration     = "Location Code Administration";
        public static string RPCAdministration              = "RPC Administration";
        public static string DataModuleTypeAdministration   = "Data Module Type Administration";

        // ── Manage module ────────────────────────────────────────────────────
        public static string SNSAdministration              = "SNS Administration";
        public static string ProjectAdministration          = "Project Administration";
        public static string DMCAdministration              = "DMC Administration";
        public static string AllocationAdministration       = "Allocation Administration";
        public static string StylesheetAdministration       = "Stylesheet Administration";
        public static string ImagesAdministration           = "Images Administration";

        // ── ICN module ───────────────────────────────────────────────────────
        public static string GenerateICNNumber              = "Generate ICN";
        public static string ViewICNNumber                  = "View ICN";

        // ── Publisher module — granular per-tab permissions (new) ────────────
        // Values follow "Module: Feature" convention; older coarse "Publisher"
        // claim is kept for any roles that already hold it.
        public static string Publisher                      = "Publisher";
        public static string PublisherIetp                  = "Publisher: IETP";
        public static string PublisherPdf                   = "Publisher: PDF";
        public static string PublisherLicense               = "Publisher: License";

        // ── Viewer module ────────────────────────────────────────────────────
        public static string Viewer                         = "Viewer";

        // ── Licensing module ─────────────────────────────────────────────────
        public static string Licensing                      = "Licensing";

        // ── Administration (restricted — admin-only) ─────────────────────────
        public static string UserAndRoleAdministration      = "User and Role Administration";
    }
}
