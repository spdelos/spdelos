using CSDBPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor kept for migration tooling; OnConfiguring falls back
        // to appsettings.json only when no options are already configured (i.e. at design time).
        public ApplicationDbContext()
        {
        }

        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; } = null!;
        public virtual DbSet<BrexRule> BrexRules { get; set; } = null!;
        public virtual DbSet<CompanyInformation> CompanyInformations { get; set; } = null!;
        public virtual DbSet<DataModule> DataModules { get; set; } = null!;
        public virtual DbSet<DataModuleStatus> DataModuleStatuses { get; set; } = null!;
        public virtual DbSet<DataModuleType> DataModuleTypes { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<IcnNumber> IcnNumbers { get; set; } = null!;
        public virtual DbSet<ICNFormatField> ICNFormatFields { get; set; } = null!;
        public virtual DbSet<ICNFormatMasterField> ICNFormatMasterFields { get; set; } = null!;
        public virtual DbSet<Icnformat> Icnformats { get; set; } = null!;
        public virtual DbSet<InformationCode> InformationCodes { get; set; } = null!;
        public virtual DbSet<InformationCodeSet> InformationCodeSets { get; set; } = null!;
        public virtual DbSet<IssueNo> IssueNos { get; set; } = null!;
        public virtual DbSet<IssueTypeFile> IssueTypeFiles { get; set; } = null!;
        public virtual DbSet<LicenseManagement> LicenseManagements { get; set; } = null!;
        public virtual DbSet<LocationCode> LocationCodes { get; set; } = null!;
        public virtual DbSet<LocationCodeSet> LocationCodeSets { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectStandardNumberingSystem> ProjectStandardNumberingSystems { get; set; } = null!;
        public virtual DbSet<ProjectNavigation> ProjectNavigations { get; set; } = null!;
        public virtual DbSet<ResponsiblePartnerCode> ResponsiblePartnerCodes { get; set; } = null!;
        public virtual DbSet<StandardNumberingSystem> StandardNumberingSystems { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;
        public virtual DbSet<XmlValidation> XmlValidations { get; set; } = null!;
        public virtual DbSet<DataModuleCode> DataModuleCodes { get; set; } = null!;
        public virtual DbSet<Stylesheet> Stylesheets { get; set; } = null!;
        public virtual DbSet<ImageAsset> ImageAssets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                var connectionString = configuration.GetSection("ConnectionStrings")
                    .GetSection("DefaultConnection").Value;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DataModuleCode — most-queried table: filtered by project, deletion flag, info/location codes
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => d.ProjectId).HasDatabaseName("IX_DataModuleCode_ProjectId");
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => d.InformationCodeId).HasDatabaseName("IX_DataModuleCode_InformationCodeId");
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => d.LocationCodeId).HasDatabaseName("IX_DataModuleCode_LocationCodeId");
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => d.IsDeleted).HasDatabaseName("IX_DataModuleCode_IsDeleted");
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => new { d.ProjectId, d.IsDeleted }).HasDatabaseName("IX_DataModuleCode_ProjectId_IsDeleted");
            modelBuilder.Entity<DataModuleCode>().HasIndex(d => new { d.ProjectId, d.IsBrexXml }).HasDatabaseName("IX_DataModuleCode_ProjectId_IsBrexXml");

            // ProjectNavigation — tree queries always filter by ProjectId + ParentId
            modelBuilder.Entity<ProjectNavigation>().HasIndex(n => n.ProjectId).HasDatabaseName("IX_ProjectNavigation_ProjectId");
            modelBuilder.Entity<ProjectNavigation>().HasIndex(n => n.ParentId).HasDatabaseName("IX_ProjectNavigation_ParentId");
            modelBuilder.Entity<ProjectNavigation>().HasIndex(n => new { n.ProjectId, n.ParentId }).HasDatabaseName("IX_ProjectNavigation_ProjectId_ParentId");

            // StandardNumberingSystem — self-join on ParentId for tree traversal
            modelBuilder.Entity<StandardNumberingSystem>().HasIndex(s => s.ParentId).HasDatabaseName("IX_StandardNumberingSystem_ParentId");

            // LocationCode — joined on LocationCodeSetId in every location-code query
            modelBuilder.Entity<LocationCode>().HasIndex(l => l.LocationCodeSetId).HasDatabaseName("IX_LocationCode_LocationCodeSetId");

            // InformationCode — joined on InformationCodeSetId in every info-code query
            modelBuilder.Entity<InformationCode>().HasIndex(i => i.InformationCodeSetId).HasDatabaseName("IX_InformationCode_InformationCodeSetId");

            // Project — FK columns used in joins from DMC and ICN queries
            modelBuilder.Entity<Project>().HasIndex(p => p.IcnformatId).HasDatabaseName("IX_Project_IcnformatId");
            modelBuilder.Entity<Project>().HasIndex(p => p.InformationCodeId).HasDatabaseName("IX_Project_InformationCodeId");
            modelBuilder.Entity<Project>().HasIndex(p => p.IssueNoId).HasDatabaseName("IX_Project_IssueNoId");

            // IcnNumber — filtered and max-aggregated by ProjectId and SeqNo
            modelBuilder.Entity<IcnNumber>().HasIndex(i => i.ProjectId).HasDatabaseName("IX_IcnNumber_ProjectId");
            modelBuilder.Entity<IcnNumber>().HasIndex(i => new { i.ProjectId, i.SeqNo }).HasDatabaseName("IX_IcnNumber_ProjectId_SeqNo");

            // BrexRule — always filtered by ProjectId
            modelBuilder.Entity<BrexRule>().HasIndex(b => b.ProjectId).HasDatabaseName("IX_BrexRule_ProjectId");

            // ProjectStandardNumberingSystem — filtered by ProjectId and joined on Snsid
            modelBuilder.Entity<ProjectStandardNumberingSystem>().HasIndex(p => p.ProjectId).HasDatabaseName("IX_ProjectSNS_ProjectId");
            modelBuilder.Entity<ProjectStandardNumberingSystem>().HasIndex(p => p.Snsid).HasDatabaseName("IX_ProjectSNS_Snsid");
            modelBuilder.Entity<ProjectStandardNumberingSystem>().HasIndex(p => new { p.ProjectId, p.Snsid }).HasDatabaseName("IX_ProjectSNS_ProjectId_Snsid");

            // IssueTypeFile — filtered by IssueNoId
            modelBuilder.Entity<IssueTypeFile>().HasIndex(i => i.IssueNoId).HasDatabaseName("IX_IssueTypeFile_IssueNoId");

            // ICNFormatField — joined and ordered by ICNFormatId
            modelBuilder.Entity<ICNFormatField>().HasIndex(i => i.ICNFormatId).HasDatabaseName("IX_ICNFormatField_ICNFormatId");
        }
    }
}
