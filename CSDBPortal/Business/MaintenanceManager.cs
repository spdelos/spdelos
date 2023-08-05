using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Xml;
using Project = CSDBPortal.Models.Project;

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
                                      p.RPCId,
                                      p.TrackPercentComplete,
                                      p.CreateDefaultBrex,
                                      p.CreatedBy,
                                      p.CreatedDate,
                                      p.ModifiedBy,
                                      p.ModifiedDate,
                                      p.NavigationXml,
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
                            RPCId = item.RPCId,
                            TrackPercentComplete = item.TrackPercentComplete,
                            CreateDefaultBrex = item.CreateDefaultBrex,
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate,
                            ModifiedBy = item.ModifiedBy,
                            ModifiedDate = item.ModifiedDate,
                            InformationCodeProp = item.InformationCodeSetDescription,
                            ICNFormatDescription = item.ICNDescription,
                            Variant = item.Variant,
                            NavigationXml = item.NavigationXml
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

                    var dataModuleCodes = (from dmc in applicationDbContext.DataModuleCodes
                                    join p in applicationDbContext.Projects on dmc.ProjectId equals p.Id
                                    join ic in applicationDbContext.InformationCodes on dmc.InformationCodeId equals ic.Id
                                    join lc in applicationDbContext.LocationCodes on dmc.LocationCodeId equals lc.Id
                                    where dmc.IsDeleted == false
                                    select new {
                                        dmc.Id,
                                        dmc.DMC,
                                        dmc.ProjectId,
                                        dmc.ModelIdentification,
                                        dmc.SDC,
                                        dmc.StandardNumberingSystem,
                                        dmc.DCV,
                                        dmc.InformationCodeId,
                                        dmc.ICV,
                                        dmc.InfoName,
                                        dmc.TechName,
                                        dmc.LocationCodeId,
                                        dmc.CreatedBy,
                                        dmc.IsBrexXml,
                                        dmc.CreatedOn,
                                        dmc.UpdatedBy,
                                        dmc.UpdatedOn,
                                        ProjectName = p.Title,
                                        InformationCodeDesc = ic.Code,
                                        dmc.xml,
                                        LocationCodeDesc = lc.Code}).ToList();

                    maintenanceViewModel.DataModuleCodes = new List<CustomDataModuleCode>();
                    foreach (var dataModuleCode in dataModuleCodes)
                    {
                        maintenanceViewModel.DataModuleCodes.Add(new CustomDataModuleCode()
                        {
                            CreatedBy = dataModuleCode.CreatedBy,
                            CreatedOn = dataModuleCode.CreatedOn,
                            DCV = dataModuleCode.DCV,
                            DMC = dataModuleCode.DMC,
                            ICV = dataModuleCode.ICV,
                            Id = dataModuleCode.Id,
                            InformationCodeDesc = dataModuleCode.InformationCodeDesc,
                            InformationCodeId = dataModuleCode.InformationCodeId,
                            LocationCodeDesc = dataModuleCode.LocationCodeDesc,
                            LocationCodeId = dataModuleCode.LocationCodeId,
                            ModelIdentification = dataModuleCode.ModelIdentification,
                            ProjectId = dataModuleCode.ProjectId,
                            ProjectName = dataModuleCode.ProjectName,
                            InfoName = dataModuleCode.InfoName,
                            TechName = dataModuleCode.TechName,
                            IsBrexXml = dataModuleCode.IsBrexXml,
                            StandardNumberingSystem = dataModuleCode.StandardNumberingSystem,
                            SDC = dataModuleCode.SDC,
                            xml = dataModuleCode.xml,
                            UpdatedBy = dataModuleCode.UpdatedBy,
                            UpdatedOn = dataModuleCode.UpdatedOn
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

        private List<CustomProjectNavigation> OrderTreeProjectSns(CustomProjectNavigation projectNavigation, List<CustomProjectNavigation> projectNavigations)
        {
            List<CustomProjectNavigation> newProjectNavigations = new List<CustomProjectNavigation>();
            IEnumerable<CustomProjectNavigation> projectNavigations_new = projectNavigations.Where(s => s.parent_id == projectNavigation.Id);

            newProjectNavigations.Add(projectNavigation);
            if (projectNavigations_new.Any())
            {
                foreach (CustomProjectNavigation projectNavigation1 in projectNavigations_new)
                {
                    newProjectNavigations.AddRange(OrderTreeProjectSns(projectNavigation1, projectNavigations));
                }
            }

            return newProjectNavigations;
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

        private int GetAncisterCountProjectNavigation(CustomProjectNavigation projectNavigation, List<CustomProjectNavigation> projectNavigations)
        {
            int count = 0;
            if (projectNavigation.ParentId > 0)
            {
                CustomProjectNavigation parentNavigation = projectNavigations.Where(n => n.Id == projectNavigation.ParentId).FirstOrDefault();
                count += GetAncisterCountProjectNavigation(parentNavigation, projectNavigations);
            }

            return ++count;
        }

        private void AssignLevelProjectNavigation(List<CustomProjectNavigation> projectNavigations)
        {
            foreach (CustomProjectNavigation projectNavigation in projectNavigations)
            {
                projectNavigation.level = GetAncisterCountProjectNavigation(projectNavigation, projectNavigations);
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

        public int CheckProjectSns(ProjectStandardNumberingSystem projectSns)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.ProjectStandardNumberingSystems.Where(a => a.Code == projectSns.Code && a.ProjectId == projectSns.ProjectId).Count();
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public int CheckDMC(DataModuleCode dataModuleCode)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.DataModuleCodes.Where(d => d.DMC == dataModuleCode.DMC).Count();
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

        public int CheckProject(Project project, out string brexTemplate)
        {
            int result = 0;
            brexTemplate = string.Empty;
            try
            {
                using (ApplicationDbContext ApplicationDbContext = new())
                {
                    Project newProject = ApplicationDbContext.Projects.Where(p => p.Name == project.Name).FirstOrDefault();
                    if (newProject != null)
                    {
                        brexTemplate = newProject.BrexTemplate;
                        result = 1;
                    }
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

        private void SaveProjectNavigationTreeRecursive(int projectId, NavigationTreeData[] children, int parentId, ApplicationDbContext context, string userName, XmlDocument doc, XmlElement parentNode)
        {
            if (children != null)
            {
                foreach (NavigationTreeData navTree in children)
                {
                    var datamoduleCode = context.DataModuleCodes.Where(d => d.Id == navTree.dmcId).FirstOrDefault();
                    var projectNavigation = new ProjectNavigation()
                    {
                        Id = 0,
                        ParentId = parentId,
                        ProjectId = projectId,
                        DMCId = navTree.dmcId,
                        Title = navTree.title,
                        CreatedBy = userName,
                        CreatedOn = DateTime.UtcNow,
                    };
                    context.Add(projectNavigation);
                    context.SaveChanges();

                    XmlElement siteMapNode = doc.CreateElement(string.Empty, "siteMapNode", string.Empty);
                    siteMapNode.SetAttribute("title", navTree.name);

                    if (!string.IsNullOrEmpty(datamoduleCode.xml))
                    {
                        siteMapNode.SetAttribute("url", datamoduleCode.DMC + ".xml");
                    }

                    parentNode.AppendChild(siteMapNode);

                    SaveProjectNavigationTreeRecursive(projectId, navTree.children, projectNavigation.Id, context, userName, doc, siteMapNode);
                }
            }
        }

        public bool SaveProjectNavigationTree(int projectId, NavigationTreeData[] navigationTreeData, string userName)
        {
            using (ApplicationDbContext context = new())
            {
                var projectNavigations = context.ProjectNavigations.Where(n => n.ProjectId == projectId);
                foreach (var projectNavigation in projectNavigations)
                {
                    context.Remove(projectNavigation);
                }
                context.SaveChanges();

                XmlDocument doc = new XmlDocument();

                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement siteMap = doc.CreateElement(string.Empty, "siteMap", string.Empty);
                doc.AppendChild(siteMap);

                SaveProjectNavigationTreeRecursive(projectId, navigationTreeData, 0, context, userName, doc, siteMap);

                var project = context.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                if (project != null)
                {
                    project.NavigationXml = doc.OuterXml;
                    context.SaveChanges();
                }
            }
            return true;
        }

        private List<CustomProjectNavigation> BuildNavigationTree(int projectId, int parentId, ApplicationDbContext context)
        {
            List<CustomProjectNavigation> navigationTrees = new List<CustomProjectNavigation>();

            var navigationResult = (from n in context.ProjectNavigations
                                    join dmc in context.DataModuleCodes on n.DMCId equals dmc.Id
                                    where n.ProjectId == projectId && n.ParentId == parentId
                                    select new { n.Id, n.DMCId, dmc.DMC, n.ParentId, dmc.InfoName, dmc.TechName, n.Title }).ToList();

            foreach (var navigation in navigationResult)
            {
                navigationTrees.Add(new CustomProjectNavigation()
                {
                    Id = navigation.Id,
                    ParentId = parentId,
                    DMCId = navigation.DMCId,
                    DMC = navigation.DMC,
                    level = 0,
                    parent_id = navigation.ParentId,
                    Title = navigation.Title,
                    ProjectId = projectId,
                    Children = BuildNavigationTree(projectId, navigation.Id, context)
                });
            }

            return navigationTrees;
        }

        public List<CustomProjectNavigation> GetProjectNavigationTree(int projectId)
        {
            List<CustomProjectNavigation> navigationTrees = new List<CustomProjectNavigation>();

            try
            {
                using (ApplicationDbContext context = new())
                {
                    Project project = context.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                    
                    if (project != null)
                    {
                        var projectNavigations = context.ProjectNavigations.Where(p => p.ProjectId == projectId).ToList();

                        if (projectNavigations.Count <= 0)
                        {
                            var dataModelCodes = context.DataModuleCodes.Where(p => p.ProjectId == projectId).ToList();
                            foreach(DataModuleCode code in  dataModelCodes)
                            {
                                navigationTrees.Add(new CustomProjectNavigation()
                                {
                                    Id = 0,
                                    ParentId = 0,
                                    DMCId = code.Id,
                                    DMC = code.DMC,
                                    level = 0,
                                    parent_id = 0,
                                    Title = code.InfoName + " - " + code.TechName,
                                    ProjectId = projectId,
                                    Children = new List<CustomProjectNavigation>()
                                });
                            }
                        }
                        else
                        {
                            var trees = BuildNavigationTree(projectId, 0, context);
                            foreach(var tree in trees)
                            {
                                navigationTrees.Add(tree);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return navigationTrees;
        }

        public List<DataModuleCode> GetDataModuleCodes(int projectId)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                return applicationDbContext.DataModuleCodes.Where(d => d.ProjectId == projectId && d.IsBrexXml == false && d.IsDeleted == false).ToList();
            }
        }

        public List<CustomProjectNavigation> GetProjectNavigation(int projectId)
        {
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    var navigationResult = (from n1 in applicationDbContext.ProjectNavigations
                                     join dmc1 in applicationDbContext.DataModuleCodes on n1.DMCId equals dmc1.Id
                                     join dmc2 in applicationDbContext.DataModuleCodes on n1.ParentId equals dmc2.Id
                                     join n2 in applicationDbContext.ProjectNavigations on n1.ParentId equals n2.Id into n3
                                     from n2 in n3.DefaultIfEmpty()
                                     where n1.ProjectId == projectId
                                     select new { n1.Id, n1.DMCId, dmc1.DMC, n1.CreatedBy, n1.CreatedOn, n1.ParentId, n1.UpdatedBy, n1.UpdatedOn, ParentDMC = dmc2.DMC }).ToList();

                    List<CustomProjectNavigation> projectNavigations = new List<CustomProjectNavigation>();
                    foreach (var item in navigationResult)
                    {
                        projectNavigations.Add(new CustomProjectNavigation()
                        {
                            Id = item.Id,
                            DMCId = item.DMCId,
                            DMC = item.DMC,
                            ParentDMC = item.ParentDMC,
                            CreatedBy = item.CreatedBy,
                            CreatedOn = item.CreatedOn,
                            ParentId = item.ParentId,
                            UpdatedBy = item.UpdatedBy,
                            UpdatedOn = item.UpdatedOn,
                            
                            parent_id = item.ParentId,
                            Title = item.DMC
                        });
                    }

                    AssignLevelProjectNavigation(projectNavigations);

                    List<CustomProjectNavigation> newProjectNavigations = new List<CustomProjectNavigation>();
                    IEnumerable<CustomProjectNavigation> rootElements = projectNavigations.Where(s => s.parent_id == 0);
                    foreach (CustomProjectNavigation projectNavigation in rootElements)
                    {
                        newProjectNavigations.AddRange(OrderTreeProjectSns(projectNavigation, projectNavigations));
                    }

                    return newProjectNavigations;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                            Id = item.Snsid.Value,
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

        public List<LocationCode> GetLocationCodes(int projectId)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                List<LocationCode> locationCodes = new List<LocationCode>();
                Project project = applicationDbContext.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                if (project != null)
                {
                    locationCodes = applicationDbContext.LocationCodes.Where(l => l.LocationCodeSetId == project.LocationCodeId).ToList();
                }

                return locationCodes;
            }
        }

        public List<InformationCode> GetInformationCodes(int projectId)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                List<InformationCode> inforamtionCodes = new List<InformationCode>();
                Project project = applicationDbContext.Projects.Where(p => p.Id == projectId).FirstOrDefault();
                if (project != null)
                {
                    inforamtionCodes = applicationDbContext.InformationCodes.Where(i => i.InformationCodeSetId == project.InformationCodeId).ToList();
                }

                return inforamtionCodes;
            }
        }

        public Project GetProject(int projectId)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                return applicationDbContext.Projects.Where(p => p.Id == projectId).FirstOrDefault();
            }
        }

        public string GetSnsCode(int snsId, int projectId)
        {
            using (ApplicationDbContext applicationDbContext = new())
            {
                string snsCode = string.Empty;
                int depth = 0;
                ProjectStandardNumberingSystem sns = applicationDbContext.ProjectStandardNumberingSystems.Where(s => s.Snsid == snsId).FirstOrDefault();

                GetSnsCode(sns.Snsid.Value, projectId, applicationDbContext, out snsCode, out depth);
                int nodesToAdd = 4 - depth % 4;

                if (nodesToAdd < 4)
                {
                    for (int count = 0; count < nodesToAdd; count++)
                    {
                        snsCode = snsCode + "00-";
                    }
                }

                snsCode = snsCode.Substring(0, snsCode.Length - 1);

                return snsCode;
            }
        }

        private void GetSnsCode(int snsId, int projectId, ApplicationDbContext context, out string snsCode, out int depth)
        {
            snsCode = string.Empty;
            depth = 0;
            ProjectStandardNumberingSystem sns = context.ProjectStandardNumberingSystems.Where(s => s.Snsid == snsId && s.ProjectId == projectId).FirstOrDefault();
            if (sns.ParentId > 0)
            {
                GetSnsCode(sns.ParentId, projectId, context, out snsCode, out depth);
                snsCode = snsCode + sns.Code + "-";
                depth++;
            }
            else
            {
                snsCode = snsCode + sns.Code + "-";
                depth++;
            }
        }
    }
}
