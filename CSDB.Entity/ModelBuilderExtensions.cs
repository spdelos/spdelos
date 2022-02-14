// Decompiled with JetBrains decompiler
// Type: CSDB.Entity.ModelBuilderExtensions
// Assembly: CSDB.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F87B454-8F51-476B-A340-3FC2E4AAC63A
// Assembly location: C:\Users\asus\Downloads\123\publish\CSDB.Entity.dll

using CSDB.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSDB.Entity
{
  public static class ModelBuilderExtensions
  {
    public static void Seed(this ModelBuilder builder)
    {
      List<IdentityRole> identityRoleList1 = new List<IdentityRole>();
      IdentityRole identityRole1 = new IdentityRole();
      identityRole1.Id = Guid.NewGuid().ToString();
      identityRole1.Name = "SuperAdmin";
      identityRole1.NormalizedName = "SuperAdmin";
      identityRoleList1.Add(identityRole1);
      IdentityRole identityRole2 = new IdentityRole();
      identityRole2.Id = Guid.NewGuid().ToString();
      identityRole2.Name = "Manager";
      identityRole2.NormalizedName = "Manager";
      identityRoleList1.Add(identityRole2);
      IdentityRole identityRole3 = new IdentityRole();
      identityRole3.Id = Guid.NewGuid().ToString();
      identityRole3.Name = "User";
      identityRole3.NormalizedName = "User";
      identityRoleList1.Add(identityRole3);
      List<IdentityRole> identityRoleList2 = identityRoleList1;
      builder.Entity<IdentityRole>().HasData((IEnumerable<IdentityRole>) identityRoleList2);
      builder.Entity<CompanyInformation>().HasData((IEnumerable<CompanyInformation>) new List<CompanyInformation>()
      {
        new CompanyInformation()
        {
          CompanyId = 1,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Location = "Hyderabad",
          Name = "Default",
          SharedPath = "D:\\sharedpath",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = new DateTime?(DateTime.Now),
          Year = DateTime.Now.Year.ToString()
        }
      });
      builder.Entity<Designation>().HasData((IEnumerable<Designation>) new List<Designation>()
      {
        new Designation() { Id = 1, Name = "Manager" },
        new Designation() { Id = 2, Name = "Employee" }
      });
      PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
      List<ApplicationUser> applicationUserList = new List<ApplicationUser>();
      ApplicationUser applicationUser1 = new ApplicationUser();
      applicationUser1.UserName = "vamshitumu@gmail.com";
      applicationUser1.Email = "vamshitumu@gmail.com";
      applicationUser1.NormalizedUserName = "vamshitumu@gmail.com".ToUpper();
      applicationUser1.NormalizedEmail = "vamshitumu@gmail.com".ToUpper();
      applicationUser1.EmailConfirmed = true;
      applicationUser1.PasswordHash = "P@$$w0rd@2021";
      applicationUser1.SecurityStamp = string.Empty;
      applicationUserList.Add(applicationUser1);
      ApplicationUser applicationUser2 = new ApplicationUser();
      applicationUser2.Id = Guid.NewGuid().ToString();
      applicationUser2.UserName = "manager@comp.com";
      applicationUser2.Email = "manager@comp.com";
      applicationUser2.NormalizedUserName = "manager@comp.com".ToUpper();
      applicationUser2.NormalizedEmail = "manager@comp.com".ToUpper();
      applicationUser2.EmailConfirmed = true;
      applicationUser2.PasswordHash = "P@$$w0rd@2021";
      applicationUser2.SecurityStamp = string.Empty;
      applicationUserList.Add(applicationUser2);
      ApplicationUser applicationUser3 = new ApplicationUser();
      applicationUser3.Id = Guid.NewGuid().ToString();
      applicationUser3.UserName = "user@comp.com";
      applicationUser3.Email = "user@comp.com";
      applicationUser3.NormalizedUserName = "user@comp.com".ToUpper();
      applicationUser3.NormalizedEmail = "user@comp.com".ToUpper();
      applicationUser3.EmailConfirmed = true;
      applicationUser3.PasswordHash = "P@$$w0rd@2021";
      applicationUser3.SecurityStamp = string.Empty;
      applicationUserList.Add(applicationUser3);
      List<ApplicationUser> data1 = applicationUserList;
      builder.Entity<ApplicationUser>().HasData((IEnumerable<ApplicationUser>) data1);
      List<IdentityUserRole<string>> data2 = new List<IdentityUserRole<string>>();
      data1[0].PasswordHash = passwordHasher.HashPassword(data1[0], "P@$$w0rd@2021");
      data1[1].PasswordHash = passwordHasher.HashPassword(data1[1], "P@$$w0rd@2021");
      data1[2].PasswordHash = passwordHasher.HashPassword(data1[2], "P@$$w0rd@2021");
      data2.Add(new IdentityUserRole<string>()
      {
        UserId = data1[0].Id,
        RoleId = identityRoleList2.First<IdentityRole>((Func<IdentityRole, bool>) (q => q.Name == "SuperAdmin")).Id
      });
      data2.Add(new IdentityUserRole<string>()
      {
        UserId = data1[1].Id,
        RoleId = identityRoleList2.First<IdentityRole>((Func<IdentityRole, bool>) (q => q.Name == "Manager")).Id
      });
      data2.Add(new IdentityUserRole<string>()
      {
        UserId = data1[2].Id,
        RoleId = identityRoleList2.First<IdentityRole>((Func<IdentityRole, bool>) (q => q.Name == "User")).Id
      });
      builder.Entity<UserDetails>().HasData((IEnumerable<UserDetails>) new List<UserDetails>()
      {
        new UserDetails()
        {
          CompanyId = 1,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          DesignationFk = new int?(1),
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          UserDetailId = 1,
          UserId = data1[1].Id
        },
        new UserDetails()
        {
          CompanyId = 1,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          DesignationFk = new int?(2),
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          UserDetailId = 2,
          UserId = data1[2].Id
        }
      });
      builder.Entity<IssueNo>().HasData((IEnumerable<IssueNo>) new List<IssueNo>()
      {
        new IssueNo()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreateOn = DateTime.Now,
          Id = 1,
          IsDelete = false,
          Name = "Issue 3.0",
          Path = "\\Xml\\3.0"
        },
        new IssueNo()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreateOn = DateTime.Now,
          Id = 2,
          IsDelete = false,
          Name = "Issue 4.0",
          Path = "\\Xml\\4.0"
        },
        new IssueNo()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreateOn = DateTime.Now,
          Id = 3,
          IsDelete = false,
          Name = "Issue 4.2",
          Path = "\\Xml\\4.2"
        },
        new IssueNo()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreateOn = DateTime.Now,
          Id = 4,
          IsDelete = false,
          Name = "Issue 5.0",
          Path = "\\Xml\\5.0"
        }
      });
      builder.Entity<IdentityUserRole<string>>().HasData((IEnumerable<IdentityUserRole<string>>) data2);
      builder.Entity<DataModuleType>().HasData((IEnumerable<DataModuleType>) new List<DataModuleType>()
      {
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "appliccrossreftable.xml",
          Name = "Applicability Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 1,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brex.xml",
          Name = "Brex",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 2,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "comment.xml",
          Name = "Comment",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 3,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "condcrossreftable.xml",
          Name = "Conditions Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 4,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "container.xml",
          Name = "Container Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 5,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "crew.xml",
          Name = "Crew/ Operator",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 6,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ddn.xml",
          Name = "DDN",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 7,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "descript.xml",
          Name = "Descriptive",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 8,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "dml.xml",
          Name = "Data Module Requirement List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 9,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "fault.xml",
          Name = "Fault",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 10,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ipd.xml",
          Name = "Illustrated Parts List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 11,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "pm.xml",
          Name = "Publication Module",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 12,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "prdcrossreftable.xml",
          Name = "Product Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 13,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "proced.xml",
          Name = "Procedural",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 14,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "process.xml",
          Name = "Process",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 15,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "rdf.xml",
          Name = "RDF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 16,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "schedul.xml",
          Name = "Maintenance Planning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 17,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "techrep.xml",
          Name = "Technical Repository",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 18,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngdata.xml",
          Name = "Wiring Data Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 19,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngflds.xml",
          Name = "Wiring Data Description",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 20,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "xcf.xml",
          Name = "XCF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 21,
          IssueNo = 1
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "appliccrossreftable.xml",
          Name = "Applicability Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 22,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brex.xml",
          Name = "Brex",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 23,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "checklist.xml",
          Name = "Check List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 24,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "comment.xml",
          Name = "Comment",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 25,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "condcrossreftable.xml",
          Name = "Conditions Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 26,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "container.xml",
          Name = "Container Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 27,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "crew.xml",
          Name = "Crew/ Operator",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 28,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ddn.xml",
          Name = "DDN",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 29,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "descript.xml",
          Name = "Descriptive",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 30,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "dml.xml",
          Name = "Data Module Requirement List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 31,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "fault.xml",
          Name = "Fault",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 32,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ipd.xml",
          Name = "Illustrated Parts List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 33,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "learning.xml",
          Name = "Learning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 34,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "pm.xml",
          Name = "Publication Module",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 35,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "prdcrossreftable.xml",
          Name = "Product Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 36,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "proced.xml",
          Name = "Procedural",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 37,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "process.xml",
          Name = "Process",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 38,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "rdf.xml",
          Name = "RDF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 39,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "schedul.xml",
          Name = "Maintenance Planning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 40,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "scormcontentpackage.xml",
          Name = "Scorm Content",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 41,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "techrep.xml",
          Name = "Technical Repository",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 42,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngdata.xml",
          Name = "Wiring Data Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 43,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngflds.xml",
          Name = "Wiring Data Description",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 44,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "xcf.xml",
          Name = "XCF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 45,
          IssueNo = 2
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "appliccrossreftable.xml",
          Name = "Applicability Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 46,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brdoc.xml",
          Name = "BRDOC",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 47,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brex.xml",
          Name = "Brex",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 48,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "checklist.xml",
          Name = "Check List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 49,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "comment.xml",
          Name = "Comment",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 50,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "comrep.xml",
          Name = "COMREP",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 51,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "condcrossreftable.xml",
          Name = "Conditions Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 52,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "container.xml",
          Name = "Container Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 53,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "crew.xml",
          Name = "Crew/ Operator",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 54,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ddn.xml",
          Name = "DDN",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 55,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "descript.xml",
          Name = "Descriptive",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 56,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "dml.xml",
          Name = "Data Module Requirement List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 57,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "fault.xml",
          Name = "Fault",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 58,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "frontmatter.xml",
          Name = "Front Matter",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 59,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "icnmetadata.xml",
          Name = "ICN Metadata",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 60,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ipd.xml",
          Name = "Illustrated Parts List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 61,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "learning.xml",
          Name = "Learning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 62,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "pm.xml",
          Name = "PM",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 63,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "prdcrossreftable.xml",
          Name = "Product Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 64,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "proced.xml",
          Name = "Procedural",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 65,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "process.xml",
          Name = "Process",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 66,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "rdf.xml",
          Name = "RDF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 67,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "sb.xml",
          Name = "SB",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 68,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "schedul.xml",
          Name = "Maintenance Planning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 69,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "scormcontentpackage.xml",
          Name = "Scorm Content",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 70,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "scocontent.xml",
          Name = "Sco Content",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 71,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "update.xml",
          Name = "Update",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 72,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngdata.xml",
          Name = "Wiring Data Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 73,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngflds.xml",
          Name = "Wiring Data Description",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 74,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "xcf.xml",
          Name = "XCF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 75,
          IssueNo = 3
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "appliccrossreftable.xml",
          Name = "Applicability Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 76,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brdoc.xml",
          Name = "BRDOC",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 77,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "brex.xml",
          Name = "Brex",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 78,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "checklist.xml",
          Name = "Check List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 79,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "comment.xml",
          Name = "Comment",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 80,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "condcrossreftable.xml",
          Name = "Conditions Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 81,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "conrep.xml",
          Name = "CONREP",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 82,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "container.xml",
          Name = "Container Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 83,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "crew.xml",
          Name = "Crew/ Operator",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 84,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ddn.xml",
          Name = "DDN",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 85,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "descript.xml",
          Name = "Descriptive",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 86,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "dml.xml",
          Name = "Data Module Requirement List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 87,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "fault.xml",
          Name = "Fault",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 88,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "frontmatter.xml",
          Name = "Front Matter",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 89,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "icnmetadata.xml",
          Name = "ICN Meta Data",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 90,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "ipd.xml",
          Name = "Illustrated Parts List",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 91,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "learning.xml",
          Name = "Learning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 92,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "pm.xml",
          Name = "Publication Module",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 93,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "prdcrossreftable.xml",
          Name = "Product Cross Ref",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 94,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "proced.xml",
          Name = "Procedural",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 95,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "process.xml",
          Name = "Process",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 96,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "rdf.xml",
          Name = "RDF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 97,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "sb.xml",
          Name = "SB",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 98,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "schedul.xml",
          Name = "Maintenance Planning",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 99,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "scocontent.xml",
          Name = "SCO Content",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 100,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "scormcontentpackage.xml",
          Name = "Scorm Content",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 101,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "update.xml",
          Name = "Update",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 102,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngdata.xml",
          Name = "Wiring Data Information",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 103,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "wrngflds.xml",
          Name = "Wiring Data Description",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 104,
          IssueNo = 4
        },
        new DataModuleType()
        {
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          FileName = "xcf.xml",
          Name = "XCF",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 105,
          IssueNo = 4
        }
      });
      builder.Entity<ResponsiblePartnerCode>().HasData((IEnumerable<ResponsiblePartnerCode>) new List<ResponsiblePartnerCode>()
      {
        new ResponsiblePartnerCode()
        {
          Code = "0",
          OrginatorCage = "U8033",
          RPCCage = "U8033",
          Id = 1
        }
      });
      builder.Entity<LocationCode>().HasData((IEnumerable<LocationCode>) new List<LocationCode>()
      {
        new LocationCode()
        {
          Code = "A",
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Description = "A",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 1
        },
        new LocationCode()
        {
          Code = "B",
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Description = "B",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 2
        },
        new LocationCode()
        {
          Code = "C",
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Description = "C",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 3
        },
        new LocationCode()
        {
          Code = "D",
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Description = "D",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 4
        }
      });
      builder.Entity<ICNFormat>().HasData((IEnumerable<ICNFormat>) new List<ICNFormat>()
      {
        new ICNFormat()
        {
          Code = "520",
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          Description = "ICN Model identification code based format",
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now,
          Id = 5
        }
      });
      builder.Entity<BrexRules>().HasData((IEnumerable<BrexRules>) new List<BrexRules>()
      {
        new BrexRules()
        {
          Id = 1,
          Group = "General",
          RuleName = "BREX-S1-00231",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmStatus",
          SubXmlTag = "security",
          Type = "Range",
          Length = 0,
          RangeValue = "01-09",
          MatchValue = "",
          AttributeName = "securityClassification",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 2,
          Group = "General",
          RuleName = "BREX-S1-00204",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = " levelledPara",
          SubXmlTag = "internalRef",
          Type = "Range",
          Length = 0,
          RangeValue = " irtt01-irtt16",
          MatchValue = "",
          AttributeName = "internalRefTargetType",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 3,
          Group = "General",
          RuleName = "BREX-S1-00182",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "acronym",
          SubXmlTag = "",
          Type = "Range",
          Length = 0,
          RangeValue = "at01-at04",
          MatchValue = "",
          AttributeName = "acronymType",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 4,
          Group = "General",
          RuleName = "BREX-S1-00148",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmCode",
          SubXmlTag = "",
          Type = "Length",
          Length = 4,
          RangeValue = "",
          MatchValue = "",
          AttributeName = "assyCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 5,
          Group = "General",
          RuleName = "BREX-S1-00147",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmIdent",
          SubXmlTag = "dmCode",
          Type = "Length",
          Length = 1,
          RangeValue = "",
          MatchValue = "",
          AttributeName = "subSubSystemCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 6,
          Group = "General",
          RuleName = "BREX-S1-00146",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmIdent",
          SubXmlTag = "dmCode",
          Type = "Length",
          Length = 1,
          RangeValue = "",
          MatchValue = "",
          AttributeName = "subSystemCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 7,
          Group = "General",
          RuleName = "BREX-S1-00145",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmCode",
          SubXmlTag = "",
          Type = "Length",
          Length = 3,
          RangeValue = "",
          MatchValue = "",
          AttributeName = "modelIdentCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 8,
          Group = "General",
          RuleName = "BREX-S1-00029",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmIdent",
          SubXmlTag = "language",
          Type = "Length",
          Length = 2,
          RangeValue = "A-Z",
          MatchValue = "",
          AttributeName = "countryIsoCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 9,
          Group = "General",
          RuleName = "BREX-S1-00029",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmIdent",
          SubXmlTag = "language",
          Type = "Length",
          Length = 2,
          RangeValue = "A-Z",
          MatchValue = "",
          AttributeName = "languageIsoCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 10,
          Group = "General",
          RuleName = "BREX-S1-00030",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmodule",
          SubXmlTag = "issueInfo",
          Type = "Equals",
          Length = 0,
          RangeValue = "",
          MatchValue = "000 or 001",
          AttributeName = "issueNumber",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        },
        new BrexRules()
        {
          Id = 11,
          Group = "General",
          RuleName = "BREX-S1-00035",
          Xml = "",
          DMTYPE = "",
          IssueNoId = 0,
          XmlTag = "dmStatus",
          SubXmlTag = "responsiblePartnerCompany",
          Type = "Contains",
          Length = 0,
          RangeValue = "",
          MatchValue = "",
          AttributeName = "enterpriseCode",
          IsActive = true,
          CreatedBy = "vamshitumu@gmail.com",
          CreatedOn = DateTime.Now,
          UpdatedBy = "vamshitumu@gmail.com",
          UpdatedOn = DateTime.Now
        }
      });
    }
  }
}
