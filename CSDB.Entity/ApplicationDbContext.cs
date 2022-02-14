// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.ApplicationDbContext
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using CSDB.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSDB.Entity
{
  public class ApplicationDbContext : IdentityDbContext<IdentityUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base((DbContextOptions) options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Seed();
      base.OnModelCreating(builder);
    }

    public DbSet<CSDB.Entity.Models.ApplicationUser> ApplicationUser { get; set; }

    public DbSet<CSDB.Entity.Models.ApplicationSettings> ApplicationSettings { get; set; }

    public DbSet<CSDB.Entity.Models.IssueNo> IssueNo { get; set; }

    public DbSet<DataModuleType> DataModuleTypes { get; set; }

    public DbSet<FolderUserLink> FolderUserLinks { get; set; }

    public DbSet<FolderDetail> FolderDetails { get; set; }

    public DbSet<CSDB.Entity.Models.InformationCodes> InformationCodes { get; set; }

    public DbSet<ICNFormat> ICNFormats { get; set; }

    public DbSet<StandardNumberingSystem> StandardNumberingSystems { get; set; }

    public DbSet<CSDB.Entity.Models.CompanyInformation> CompanyInformation { get; set; }

    public DbSet<CSDB.Entity.Models.UserDetails> UserDetails { get; set; }

    public DbSet<CSDB.Entity.Models.LicenseManagement> LicenseManagement { get; set; }

    public DbSet<ProjectStandardNumberingSystem> ProjectStandardNumberingSystems { get; set; }

    public DbSet<Designation> Designations { get; set; }

    public DbSet<ResponsiblePartnerCode> ResponsiblePartnerCodes { get; set; }

    public DbSet<CSDB.Entity.Models.Project> Project { get; set; }

    public DbSet<CSDB.Entity.Models.DataModule> DataModule { get; set; }

    public DbSet<LocationCode> LocationCodes { get; set; }

    public DbSet<CSDB.Entity.Models.XmlValidation> XmlValidation { get; set; }

    public DbSet<CSDB.Entity.Models.DataModuleStatus> DataModuleStatus { get; set; }

    public DbSet<CSDB.Entity.Models.IcnNumbers> IcnNumbers { get; set; }

    public DbSet<CSDB.Entity.Models.BrexRules> BrexRules { get; set; }
  }
}
