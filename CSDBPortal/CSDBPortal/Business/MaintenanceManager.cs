using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;

namespace CSDBPortal.Business
{
    public class MaintenanceManager
    {
        public MaintenanceViewModel GetMaintenanceDetailInfo()
        {
            MaintenanceViewModel maintenanceViewModel = new MaintenanceViewModel();
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    maintenanceViewModel.LocationCodes = new List<CustomLocationCode>();
                    maintenanceViewModel.LocationCodesSets = new List<LocationCodeSet>();
                    maintenanceViewModel.ResponsiblePartnerCodes = new List<ResponsiblePartnerCode>();
                    maintenanceViewModel.DataModuleTypes = new List<DataModuleType>();
                    maintenanceViewModel.Issues = new List<IssueNo>();
                    maintenanceViewModel.Icnformats = new List<Icnformat>();
                    maintenanceViewModel.InformationCodeSets = new List<InformationCodeSet>();

                    maintenanceViewModel.LocationCodesSets = applicationDbContext.LocationCodeSets.ToList();
                    maintenanceViewModel.ResponsiblePartnerCodes = applicationDbContext.ResponsiblePartnerCodes.ToList();
                    maintenanceViewModel.DataModuleTypes = applicationDbContext.DataModuleTypes.ToList();
                    maintenanceViewModel.Issues = applicationDbContext.IssueNos.ToList();
                    maintenanceViewModel.Icnformats = applicationDbContext.Icnformats.ToList();
                    maintenanceViewModel.InformationCodeSets = applicationDbContext.InformationCodeSets.ToList();

                    var lcresult = (from lc in applicationDbContext.LocationCodes
                                  join lcs in applicationDbContext.LocationCodeSets on lc.LocationCodeSetId equals lcs.Id
                                  select new { lc.Id, lc.Code, lc.Description, lc.LocationCodeSetId, LocationCodeSetDescription = lcs.Description }).ToList();

                    foreach (var lc in lcresult)
                    {
                        maintenanceViewModel.LocationCodes.Add(new CustomLocationCode
                        {
                            Id = lc.Id,
                            Code = lc.Code,
                            Description = lc.Description,
                            LocationCodeSetId = lc.LocationCodeSetId,
                            LocationCodeSetDescription = lc.LocationCodeSetDescription
                        });
                    }

                    var result = (from p in applicationDbContext.Projects
                                  join d in applicationDbContext.InformationCodes on p.InformationCodeId equals d.Id
                                  select new { p.Name, p.ProjectCode, p.Title }).ToList();

                    maintenanceViewModel.Projects = new List<CustomProjet>();
                    foreach (var item in result)
                    {
                        maintenanceViewModel.Projects.Add(new CustomProjet {
                            Name = item.Name,
                            ProjectCode = item.ProjectCode,
                            Title = item.Title
                        });

                    }

                    var snsResult = (from s1 in applicationDbContext.StandardNumberingSystems
                                    join s2 in applicationDbContext.StandardNumberingSystems on s1.ParentId equals s2.Id into s3
                                    from s2 in s3.DefaultIfEmpty()
                                    select new { s1.Id, s1.Code, s1.Description, s1.CreatedBy, s1.CreatedOn, s1.ParentId, s1.UpdatedBy, s1.UpdatedOn, ParentCode = s2.Code }).ToList();

                    maintenanceViewModel.StandardNumberingSystems = new List<CustomStandardNumberingSystem>();
                    foreach (var item in snsResult)
                    {
                        maintenanceViewModel.StandardNumberingSystems.Add(new CustomStandardNumberingSystem()
                        {
                            Id = item.Id,
                            Code = item.Code,
                            Description = item.Description,
                            CreatedBy = item.CreatedBy,
                            CreatedOn = item.CreatedOn,
                            ParentCode = item.ParentCode,
                            ParentId = item.ParentId,
                            UpdatedBy = item.UpdatedBy,
                            UpdatedOn = item.UpdatedOn,
                        });
                    }

                    return maintenanceViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckLocationCode(LocationCode locationCode)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.LocationCodes.Where(a => a.Description == locationCode.Description).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CheckLocationCodeSet(LocationCodeSet locationCodeSet)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.LocationCodeSets.Where(l => l.Description == locationCodeSet.Description).Count();
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        public int CheckSns(StandardNumberingSystem sns)
        {
            int result = 0;
            try
            {
                using(ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.StandardNumberingSystems.Where(a => a.Code == sns.Code).Count();
                }
            }
            catch(Exception ex)
            {

            }

            return result;
        }

        public int CheckRPC(ResponsiblePartnerCode responsiblePartnerCode)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext ApplicationDbContext = new())
                {
                    result = ApplicationDbContext.ResponsiblePartnerCodes.Where(a => a.Code == responsiblePartnerCode.Code).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CheckDataModuleType(DataModuleType dataModuleType)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext ApplicationDbContext = new())
                {
                    result = ApplicationDbContext.DataModuleTypes.Where(a => a.Name == dataModuleType.Name).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CheckProject(Project project)
        {
            int result = 0;
            try
            {
                using(ApplicationDbContext ApplicationDbContext = new())
                {
                    result = ApplicationDbContext.Projects.Where(p => p.Name == project.Name).Count();
                }
            }
            catch(Exception ex)
            {
            }
            return result;
        }

    }
}
