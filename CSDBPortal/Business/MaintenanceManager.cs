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
                                  join d in applicationDbContext.InformationCodeSets on p.InformationCodeId equals d.Id
                                  join icn in applicationDbContext.Icnformats on p.IcnformatId equals icn.Id
                                  select new
                                  {
                                      p.Id,
                                      p.EndItem,
                                      p.Name,
                                      p.Title,
                                      p.IssueNoId,
                                      p.IcnformatId,
                                      p.SNSSetId,
                                      p.InformationCodeId,
                                      p.LocationCodeId,
                                      p.ModelIdentification,
                                      p.SDC,
                                      p.SubjectLength,
                                      p.ProjectCode,
                                      p.RPCId,
                                      p.TrackPercentComplete,
                                      p.CreateDefaultBrex,
                                      p.CreatedBy,
                                      p.CreatedDate,
                                      p.ModifiedBy,
                                      p.ModifiedDate,
                                      InformationCodeSetDescription = d.Description,
                                      Variant = d.Variant,
                                      ICNDescription = icn.Description
                                  }).ToList();

                    maintenanceViewModel.Projects = new List<CustomProjet>();
                    foreach (var item in result)
                    {
                        maintenanceViewModel.Projects.Add(new CustomProjet
                        {

                            Id = item.Id,
                            EndItem = item.EndItem,
                            Name = item.Name,
                            Title = item.Title,
                            IssueNoId = item.IssueNoId,
                            IcnformatId = item.IcnformatId,
                            SNSSetId = item.SNSSetId,
                            InformationCodeId = item.InformationCodeId,
                            LocationCodeId = item.LocationCodeId,
                            ModelIdentification = item.ModelIdentification,
                            SDC = item.SDC,
                            SubjectLength = item.SubjectLength,
                            ProjectCode = item.ProjectCode,
                            RPCId = item.RPCId,
                            TrackPercentComplete = item.TrackPercentComplete,
                            CreateDefaultBrex = item.CreateDefaultBrex,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate,
                            ModifiedBy = item.ModifiedBy,
                            ModifiedDate = item.ModifiedDate,
                            InformationCodeProp = item.InformationCodeSetDescription,
                            ICNFormatDescription = item.ICNDescription,
                            Variant = item.Variant
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

                            //id = item.Id,
                            parent_id = item.ParentId,
                            title = item.Code + " : " + item.Description
                        });
                    }

                    AssignLevel(maintenanceViewModel.StandardNumberingSystems);

                    List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
                    IEnumerable<CustomStandardNumberingSystem> rootElements = maintenanceViewModel.StandardNumberingSystems.Where(s => s.parent_id == 0);
                    foreach (CustomStandardNumberingSystem sns in rootElements)
                    {
                        newStandardNumbers.AddRange(OrderTree(sns, maintenanceViewModel.StandardNumberingSystems));
                    }
                    maintenanceViewModel.StandardNumberingSystems = newStandardNumbers;

                    return maintenanceViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CustomStandardNumberingSystem> OrderTree(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
            IEnumerable<CustomStandardNumberingSystem> standardNumbers = standardNumberingSystems.Where(s => s.parent_id == sns.Id);

            newStandardNumbers.Add(sns);
            if (standardNumbers.Any())
            {
                foreach(CustomStandardNumberingSystem standardNumberingSystem in standardNumbers)
                {
                    newStandardNumbers.AddRange(OrderTree(standardNumberingSystem, standardNumberingSystems));
                }
            }

            return newStandardNumbers;
        }

        private List<CustomStandardNumberingSystem> OrderTreeProjectSns(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
            IEnumerable<CustomStandardNumberingSystem> standardNumbers = standardNumberingSystems.Where(s => s.parent_id == sns.SnsId);

            newStandardNumbers.Add(sns);
            if (standardNumbers.Any())
            {
                foreach (CustomStandardNumberingSystem standardNumberingSystem in standardNumbers)
                {
                    newStandardNumbers.AddRange(OrderTreeProjectSns(standardNumberingSystem, standardNumberingSystems));
                }
            }

            return newStandardNumbers;
        }

        private int GetAncisterCount(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            int count = 0;
            if (sns.ParentId > 0)
            {
                CustomStandardNumberingSystem parentSns = standardNumberingSystems.Where(s => s.Id == sns.ParentId).FirstOrDefault();
                count += GetAncisterCount(parentSns, standardNumberingSystems);
            }

            return ++count;
        }

        private void AssignLevel(List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            foreach(CustomStandardNumberingSystem sns in standardNumberingSystems)
            {
                sns.level = GetAncisterCount(sns, standardNumberingSystems);
            }
        }

        private int GetAncisterCountProjectSns(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            int count = 0;
            if (sns.ParentId > 0)
            {
                CustomStandardNumberingSystem parentSns = standardNumberingSystems.Where(s => s.SnsId == sns.ParentId).FirstOrDefault();
                count += GetAncisterCountProjectSns(parentSns, standardNumberingSystems);
            }

            return ++count;
        }

        private void AssignLevelProject(List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            foreach (CustomStandardNumberingSystem sns in standardNumberingSystems)
            {
                sns.level = GetAncisterCountProjectSns(sns, standardNumberingSystems);
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
            catch (Exception ex)
            {

            }
            return result;
        }

        public int CheckSns(StandardNumberingSystem sns)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.StandardNumberingSystems.Where(a => a.Code == sns.Code).Count();
                }
            }
            catch (Exception ex)
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
                using (ApplicationDbContext ApplicationDbContext = new())
                {
                    result = ApplicationDbContext.Projects.Where(p => p.Name == project.Name).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        private void CopySNSRecursive(int projectId, StandardNumberingSystem sns, ApplicationDbContext context, string userName)
        {
            var snsChildren = context.StandardNumberingSystems.Where(s => s.ParentId == sns.Id);
            if (snsChildren.Any())
            {
                foreach (StandardNumberingSystem snsChild in snsChildren)
                {
                    CopySNSRecursive(projectId, snsChild, context, userName);
                }
            }

            ProjectStandardNumberingSystem projectSns = new ProjectStandardNumberingSystem()
            {
                Code = sns.Code,
                CreatedBy = userName,
                CreatedOn = DateTime.UtcNow,
                Description = sns.Description,
                ParentId = sns.ParentId,
                ProjectId = projectId,
                Snsid = sns.Id
            };
            context.ProjectStandardNumberingSystems.Add(projectSns);
        }

        public int CopySNS(Project project, string userName)
        {
            using (ApplicationDbContext context = new())
            {
                StandardNumberingSystem sns = context.StandardNumberingSystems.Where(s => s.Id == project.SNSSetId).FirstOrDefault();
                CopySNSRecursive(project.Id, sns, context, userName);
                return context.SaveChanges();
            }
        }

        public List<CustomStandardNumberingSystem> GetProjectSns(int projectId)
        {
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    var snsResult = (from s1 in applicationDbContext.ProjectStandardNumberingSystems
                                     join s2 in applicationDbContext.ProjectStandardNumberingSystems on s1.ParentId equals s2.Id into s3
                                     from s2 in s3.DefaultIfEmpty()
                                     where s1.ProjectId == projectId
                                     select new { s1.Id, s1.Code, s1.Description, s1.CreatedBy, s1.CreatedOn, s1.ParentId, s1.UpdatedBy, s1.UpdatedOn, ParentCode = s2.Code, s1.Snsid }).ToList();

                    List<CustomStandardNumberingSystem> snSystems = new List<CustomStandardNumberingSystem>();
                    foreach (var item in snsResult)
                    {
                        snSystems.Add(new CustomStandardNumberingSystem()
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
                            SnsId = item.Snsid,

                            //id = item.Id,
                            parent_id = item.ParentId,
                            title = item.Code + " : " + item.Description
                        });
                    }

                    AssignLevelProject(snSystems);

                    List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
                    IEnumerable<CustomStandardNumberingSystem> rootElements = snSystems.Where(s => s.parent_id == 0);
                    foreach (CustomStandardNumberingSystem sns in rootElements)
                    {
                        newStandardNumbers.AddRange(OrderTreeProjectSns(sns, snSystems));
                    }

                    return newStandardNumbers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
