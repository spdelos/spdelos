using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;
using Project = CSDBPortal.Models.Project;

namespace CSDBPortal.Business
{
    public class MaintenanceManager
    {
        private readonly ApplicationDbContext _db;

        public MaintenanceManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MaintenanceViewModel> GetMaintenanceDetailInfoAsync()
        {
            MaintenanceViewModel maintenanceViewModel = new MaintenanceViewModel();
            try
            {
                maintenanceViewModel.LocationCodes = new List<CustomLocationCode>();
                maintenanceViewModel.LocationCodesSets = new List<LocationCodeSet>();
                maintenanceViewModel.ResponsiblePartnerCodes = new List<ResponsiblePartnerCode>();
                maintenanceViewModel.DataModuleTypes = new List<DataModuleType>();
                maintenanceViewModel.Issues = new List<IssueNo>();
                maintenanceViewModel.Icnformats = new List<Icnformat>();
                maintenanceViewModel.InformationCodeSets = new List<InformationCodeSet>();

                maintenanceViewModel.LocationCodesSets = await _db.LocationCodeSets.ToListAsync();
                maintenanceViewModel.ResponsiblePartnerCodes = await _db.ResponsiblePartnerCodes.ToListAsync();
                maintenanceViewModel.DataModuleTypes = await _db.DataModuleTypes.ToListAsync();
                maintenanceViewModel.Issues = await _db.IssueNos.ToListAsync();
                maintenanceViewModel.Icnformats = await _db.Icnformats.ToListAsync();
                maintenanceViewModel.InformationCodeSets = await _db.InformationCodeSets.ToListAsync();

                var lcresult = await (from lc in _db.LocationCodes
                                join lcs in _db.LocationCodeSets on lc.LocationCodeSetId equals lcs.Id
                                select new { lc.Id, lc.Code, lc.Description, lc.LocationCodeSetId, LocationCodeSetDescription = lcs.Description }).ToListAsync();

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

                var result = await (from p in _db.Projects
                              join d in _db.InformationCodeSets on p.InformationCodeId equals d.Id
                              join icn in _db.Icnformats on p.IcnformatId equals icn.Id
                              select new
                              {
                                  p.Id, p.EndItem, p.Name, p.Title, p.IssueNoId, p.IcnformatId,
                                  p.SNSSetId, p.InformationCodeId, p.LocationCodeId, p.ModelIdentification,
                                  p.SDC, p.SubjectLength, p.RPCId, p.TrackPercentComplete,
                                  p.CreateDefaultBrex, p.CreatedBy, p.CreatedDate, p.ModifiedBy,
                                  p.ModifiedDate, p.NavigationXml,
                                  InformationCodeSetDescription = d.Description,
                                  Variant = d.Variant,
                                  ICNDescription = icn.Description
                              }).ToListAsync();

                maintenanceViewModel.Projects = new List<CustomProjet>();
                foreach (var item in result)
                {
                    maintenanceViewModel.Projects.Add(new CustomProjet
                    {
                        Id = item.Id, EndItem = item.EndItem, Name = item.Name, Title = item.Title,
                        IssueNoId = item.IssueNoId, IcnformatId = item.IcnformatId, SNSSetId = item.SNSSetId,
                        InformationCodeId = item.InformationCodeId, LocationCodeId = item.LocationCodeId,
                        ModelIdentification = item.ModelIdentification, SDC = item.SDC,
                        SubjectLength = item.SubjectLength, RPCId = item.RPCId,
                        TrackPercentComplete = item.TrackPercentComplete, CreateDefaultBrex = item.CreateDefaultBrex,
                        CreatedBy = item.CreatedBy, CreatedDate = item.CreatedDate,
                        ModifiedBy = item.ModifiedBy, ModifiedDate = item.ModifiedDate,
                        InformationCodeProp = item.InformationCodeSetDescription,
                        ICNFormatDescription = item.ICNDescription, Variant = item.Variant,
                        NavigationXml = item.NavigationXml
                    });
                }

                var snsResult = await (from s1 in _db.StandardNumberingSystems
                                 join s2 in _db.StandardNumberingSystems on s1.ParentId equals s2.Id into s3
                                 from s2 in s3.DefaultIfEmpty()
                                 select new { s1.Id, s1.Code, s1.Description, s1.CreatedBy, s1.CreatedOn, s1.ParentId, s1.UpdatedBy, s1.UpdatedOn, ParentCode = s2.Code }).ToListAsync();

                maintenanceViewModel.StandardNumberingSystems = new List<CustomStandardNumberingSystem>();
                foreach (var item in snsResult)
                {
                    maintenanceViewModel.StandardNumberingSystems.Add(new CustomStandardNumberingSystem()
                    {
                        Id = item.Id, Code = item.Code, Description = item.Description,
                        CreatedBy = item.CreatedBy, CreatedOn = item.CreatedOn, ParentCode = item.ParentCode,
                        ParentId = item.ParentId, UpdatedBy = item.UpdatedBy, UpdatedOn = item.UpdatedOn,
                        parent_id = item.ParentId, title = item.Code + " : " + item.Description
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

                var dataModuleCodes = await (from dmc in _db.DataModuleCodes
                                join p in _db.Projects on dmc.ProjectId equals p.Id
                                join ic in _db.InformationCodes on dmc.InformationCodeId equals ic.Id
                                join lc in _db.LocationCodes on dmc.LocationCodeId equals lc.Id
                                where dmc.IsDeleted == false
                                select new {
                                    dmc.Id, dmc.DMC, dmc.ProjectId, dmc.ModelIdentification, dmc.SDC,
                                    dmc.StandardNumberingSystem, dmc.DCV, dmc.InformationCodeId, dmc.ICV,
                                    dmc.InfoName, dmc.TechName, dmc.LocationCodeId, dmc.CreatedBy,
                                    dmc.IsBrexXml, dmc.CreatedOn, dmc.UpdatedBy, dmc.UpdatedOn,
                                    ProjectName = p.Title, InformationCodeDesc = ic.Code,
                                    dmc.xml, LocationCodeDesc = lc.Code
                                }).ToListAsync();

                maintenanceViewModel.DataModuleCodes = new List<CustomDataModuleCode>();
                foreach (var dataModuleCode in dataModuleCodes)
                {
                    maintenanceViewModel.DataModuleCodes.Add(new CustomDataModuleCode()
                    {
                        CreatedBy = dataModuleCode.CreatedBy, CreatedOn = dataModuleCode.CreatedOn,
                        DCV = dataModuleCode.DCV, DMC = dataModuleCode.DMC, ICV = dataModuleCode.ICV,
                        Id = dataModuleCode.Id, InformationCodeDesc = dataModuleCode.InformationCodeDesc,
                        InformationCodeId = dataModuleCode.InformationCodeId,
                        LocationCodeDesc = dataModuleCode.LocationCodeDesc,
                        LocationCodeId = dataModuleCode.LocationCodeId,
                        ModelIdentification = dataModuleCode.ModelIdentification,
                        ProjectId = dataModuleCode.ProjectId, ProjectName = dataModuleCode.ProjectName,
                        InfoName = dataModuleCode.InfoName, TechName = dataModuleCode.TechName,
                        IsBrexXml = dataModuleCode.IsBrexXml,
                        StandardNumberingSystem = dataModuleCode.StandardNumberingSystem,
                        SDC = dataModuleCode.SDC, xml = dataModuleCode.xml,
                        UpdatedBy = dataModuleCode.UpdatedBy, UpdatedOn = dataModuleCode.UpdatedOn
                    });
                }

                return maintenanceViewModel;
            }
            catch
            {
                throw;
            }
        }

        // ── in-memory tree helpers (no DB I/O, no async needed) ─────────────────

        private List<CustomStandardNumberingSystem> OrderTree(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
            IEnumerable<CustomStandardNumberingSystem> standardNumbers = standardNumberingSystems.Where(s => s.parent_id == sns.Id);
            newStandardNumbers.Add(sns);
            if (standardNumbers.Any())
            {
                foreach (CustomStandardNumberingSystem sn in standardNumbers)
                    newStandardNumbers.AddRange(OrderTree(sn, standardNumberingSystems));
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
                foreach (CustomStandardNumberingSystem sn in standardNumbers)
                    newStandardNumbers.AddRange(OrderTreeProjectSns(sn, standardNumberingSystems));
            }
            return newStandardNumbers;
        }

        private List<CustomProjectNavigation> OrderTreeProjectSns(CustomProjectNavigation projectNavigation, List<CustomProjectNavigation> projectNavigations)
        {
            List<CustomProjectNavigation> newProjectNavigations = new List<CustomProjectNavigation>();
            IEnumerable<CustomProjectNavigation> children = projectNavigations.Where(s => s.parent_id == projectNavigation.Id);
            newProjectNavigations.Add(projectNavigation);
            if (children.Any())
            {
                foreach (CustomProjectNavigation child in children)
                    newProjectNavigations.AddRange(OrderTreeProjectSns(child, projectNavigations));
            }
            return newProjectNavigations;
        }

        private int GetAncisterCount(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            int count = 0;
            if (sns.ParentId > 0)
            {
                CustomStandardNumberingSystem parentSns = standardNumberingSystems.FirstOrDefault(s => s.Id == sns.ParentId);
                count += GetAncisterCount(parentSns, standardNumberingSystems);
            }
            return ++count;
        }

        private void AssignLevel(List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            foreach (CustomStandardNumberingSystem sns in standardNumberingSystems)
                sns.level = GetAncisterCount(sns, standardNumberingSystems);
        }

        private int GetAncisterCountProjectNavigation(CustomProjectNavigation projectNavigation, List<CustomProjectNavigation> projectNavigations)
        {
            int count = 0;
            if (projectNavigation.ParentId > 0)
            {
                CustomProjectNavigation parentNavigation = projectNavigations.FirstOrDefault(n => n.Id == projectNavigation.ParentId);
                count += GetAncisterCountProjectNavigation(parentNavigation, projectNavigations);
            }
            return ++count;
        }

        private void AssignLevelProjectNavigation(List<CustomProjectNavigation> projectNavigations)
        {
            foreach (CustomProjectNavigation pn in projectNavigations)
                pn.level = GetAncisterCountProjectNavigation(pn, projectNavigations);
        }

        private int GetAncisterCountProjectSns(CustomStandardNumberingSystem sns, List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            int count = 0;
            if (sns.ParentId > 0)
            {
                CustomStandardNumberingSystem parentSns = standardNumberingSystems.FirstOrDefault(s => s.SnsId == sns.ParentId);
                count += GetAncisterCountProjectSns(parentSns, standardNumberingSystems);
            }
            return ++count;
        }

        private void AssignLevelProject(List<CustomStandardNumberingSystem> standardNumberingSystems)
        {
            foreach (CustomStandardNumberingSystem sns in standardNumberingSystems)
                sns.level = GetAncisterCountProjectSns(sns, standardNumberingSystems);
        }

        // ── Check methods ────────────────────────────────────────────────────────

        public async Task<int> CheckLocationCodeAsync(LocationCode locationCode)
        {
            try { return await _db.LocationCodes.CountAsync(a => a.Description == locationCode.Description); }
            catch { return 0; }
        }

        public async Task<int> CheckLocationCodeSetAsync(LocationCodeSet locationCodeSet)
        {
            try { return await _db.LocationCodeSets.CountAsync(l => l.Description == locationCodeSet.Description); }
            catch { return 0; }
        }

        public async Task<int> CheckSnsAsync(StandardNumberingSystem sns)
        {
            try { return await _db.StandardNumberingSystems.CountAsync(a => a.Code == sns.Code); }
            catch { return 0; }
        }

        public async Task<int> CheckProjectSnsAsync(ProjectStandardNumberingSystem projectSns)
        {
            try { return await _db.ProjectStandardNumberingSystems.CountAsync(a => a.Code == projectSns.Code && a.ProjectId == projectSns.ProjectId); }
            catch { return 0; }
        }

        public async Task<int> CheckDMCAsync(DataModuleCode dataModuleCode)
        {
            try { return await _db.DataModuleCodes.CountAsync(d => d.DMC == dataModuleCode.DMC); }
            catch { return 0; }
        }

        public async Task<int> CheckRPCAsync(ResponsiblePartnerCode responsiblePartnerCode)
        {
            try { return await _db.ResponsiblePartnerCodes.CountAsync(a => a.Code == responsiblePartnerCode.Code); }
            catch { return 0; }
        }

        public async Task<int> CheckDataModuleTypeAsync(DataModuleType dataModuleType)
        {
            try { return await _db.DataModuleTypes.CountAsync(a => a.Name == dataModuleType.Name); }
            catch { return 0; }
        }

        public async Task<(int result, string brexTemplate)> CheckProjectAsync(Project project)
        {
            try
            {
                Project newProject = await _db.Projects.FirstOrDefaultAsync(p => p.Name == project.Name);
                if (newProject != null)
                    return (1, newProject.BrexTemplate);
            }
            catch { }
            return (0, string.Empty);
        }

        // ── SNS copy ─────────────────────────────────────────────────────────────

        private async Task CopySNSRecursiveAsync(int projectId, StandardNumberingSystem sns, string userName)
        {
            var snsChildren = await _db.StandardNumberingSystems.Where(s => s.ParentId == sns.Id).ToListAsync();
            foreach (StandardNumberingSystem snsChild in snsChildren)
                await CopySNSRecursiveAsync(projectId, snsChild, userName);

            _db.ProjectStandardNumberingSystems.Add(new ProjectStandardNumberingSystem()
            {
                Code = sns.Code, CreatedBy = userName, CreatedOn = DateTime.UtcNow,
                Description = sns.Description, ParentId = sns.ParentId,
                ProjectId = projectId, Snsid = sns.Id
            });
        }

        public async Task<int> CopySNSAsync(Project project, string userName)
        {
            StandardNumberingSystem sns = await _db.StandardNumberingSystems.FirstOrDefaultAsync(s => s.Id == project.SNSSetId);
            await CopySNSRecursiveAsync(project.Id, sns, userName);
            return await _db.SaveChangesAsync();
        }

        // ── Navigation tree ───────────────────────────────────────────────────────

        private async Task SaveProjectNavigationTreeRecursiveAsync(int projectId, NavigationTreeData[] children, int parentId, string userName, XmlDocument doc, XmlElement parentNode)
        {
            if (children == null) return;
            foreach (NavigationTreeData navTree in children)
            {
                var datamoduleCode = await _db.DataModuleCodes.FirstOrDefaultAsync(d => d.Id == navTree.dmcId);
                var projectNavigation = new ProjectNavigation()
                {
                    Id = 0, ParentId = parentId, ProjectId = projectId, DMCId = navTree.dmcId,
                    Title = navTree.title, CreatedBy = userName, CreatedOn = DateTime.UtcNow,
                };
                _db.Add(projectNavigation);
                await _db.SaveChangesAsync();

                XmlElement siteMapNode = doc.CreateElement(string.Empty, "siteMapNode", string.Empty);
                siteMapNode.SetAttribute("title", navTree.name);
                if (!string.IsNullOrEmpty(datamoduleCode?.xml))
                    siteMapNode.SetAttribute("url", datamoduleCode.DMC + ".xml");
                parentNode.AppendChild(siteMapNode);

                await SaveProjectNavigationTreeRecursiveAsync(projectId, navTree.children, projectNavigation.Id, userName, doc, siteMapNode);
            }
        }

        public async Task<bool> SaveProjectNavigationTreeAsync(int projectId, NavigationTreeData[] navigationTreeData, string userName)
        {
            var projectNavigations = await _db.ProjectNavigations.Where(n => n.ProjectId == projectId).ToListAsync();
            _db.ProjectNavigations.RemoveRange(projectNavigations);
            await _db.SaveChangesAsync();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
            XmlElement siteMap = doc.CreateElement(string.Empty, "siteMap", string.Empty);
            doc.AppendChild(siteMap);

            await SaveProjectNavigationTreeRecursiveAsync(projectId, navigationTreeData, 0, userName, doc, siteMap);

            var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project != null)
            {
                project.NavigationXml = doc.OuterXml;
                await _db.SaveChangesAsync();
            }
            return true;
        }

        private async Task<List<CustomProjectNavigation>> BuildNavigationTreeAsync(int projectId, int parentId)
        {
            List<CustomProjectNavigation> navigationTrees = new List<CustomProjectNavigation>();

            var navigationResult = await (from n in _db.ProjectNavigations
                                    join dmc in _db.DataModuleCodes on n.DMCId equals dmc.Id
                                    where n.ProjectId == projectId && n.ParentId == parentId
                                    select new { n.Id, n.DMCId, dmc.DMC, n.ParentId, dmc.InfoName, dmc.TechName, n.Title }).ToListAsync();

            foreach (var navigation in navigationResult)
            {
                navigationTrees.Add(new CustomProjectNavigation()
                {
                    Id = navigation.Id, ParentId = parentId, DMCId = navigation.DMCId,
                    DMC = navigation.DMC, level = 0, parent_id = navigation.ParentId,
                    Title = navigation.Title, ProjectId = projectId,
                    Children = await BuildNavigationTreeAsync(projectId, navigation.Id)
                });
            }
            return navigationTrees;
        }

        public async Task<List<BrexRule>> GetBrexRulesAsync(int projectId)
        {
            return await _db.BrexRules.Where(b => b.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<CustomProjectNavigation>> GetProjectNavigationTreeAsync(int projectId)
        {
            List<CustomProjectNavigation> navigationTrees = new List<CustomProjectNavigation>();
            try
            {
                Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (project != null)
                {
                    var projectNavigations = await _db.ProjectNavigations.Where(p => p.ProjectId == projectId).ToListAsync();
                    if (projectNavigations.Count <= 0)
                    {
                        var dataModelCodes = await _db.DataModuleCodes.Where(p => p.ProjectId == projectId).ToListAsync();
                        foreach (DataModuleCode code in dataModelCodes)
                        {
                            navigationTrees.Add(new CustomProjectNavigation()
                            {
                                Id = 0, ParentId = 0, DMCId = code.Id, DMC = code.DMC, level = 0,
                                parent_id = 0, Title = code.InfoName + " - " + code.TechName,
                                ProjectId = projectId, Children = new List<CustomProjectNavigation>()
                            });
                        }
                    }
                    else
                    {
                        navigationTrees.AddRange(await BuildNavigationTreeAsync(projectId, 0));
                    }
                }
            }
            catch { }
            return navigationTrees;
        }

        public async Task<List<DataModuleCode>> GetDataModuleCodesAsync(int projectId)
        {
            return await _db.DataModuleCodes
                .Where(d => d.ProjectId == projectId && d.IsBrexXml == false && d.IsDeleted == false)
                .Select(d => new DataModuleCode { Id = d.Id, DMC = d.DMC, InfoName = d.InfoName, TechName = d.TechName })
                .ToListAsync();
        }

        public async Task<List<CustomProjectNavigation>> GetProjectNavigationAsync(int projectId)
        {
            try
            {
                var navigationResult = await (from n1 in _db.ProjectNavigations
                                 join dmc1 in _db.DataModuleCodes on n1.DMCId equals dmc1.Id
                                 join dmc2 in _db.DataModuleCodes on n1.ParentId equals dmc2.Id
                                 join n2 in _db.ProjectNavigations on n1.ParentId equals n2.Id into n3
                                 from n2 in n3.DefaultIfEmpty()
                                 where n1.ProjectId == projectId
                                 select new { n1.Id, n1.DMCId, dmc1.DMC, n1.CreatedBy, n1.CreatedOn, n1.ParentId, n1.UpdatedBy, n1.UpdatedOn, ParentDMC = dmc2.DMC }).ToListAsync();

                List<CustomProjectNavigation> projectNavigations = new List<CustomProjectNavigation>();
                foreach (var item in navigationResult)
                {
                    projectNavigations.Add(new CustomProjectNavigation()
                    {
                        Id = item.Id, DMCId = item.DMCId, DMC = item.DMC, ParentDMC = item.ParentDMC,
                        CreatedBy = item.CreatedBy, CreatedOn = item.CreatedOn, ParentId = item.ParentId,
                        UpdatedBy = item.UpdatedBy, UpdatedOn = item.UpdatedOn,
                        parent_id = item.ParentId, Title = item.DMC
                    });
                }

                AssignLevelProjectNavigation(projectNavigations);

                List<CustomProjectNavigation> newProjectNavigations = new List<CustomProjectNavigation>();
                IEnumerable<CustomProjectNavigation> rootElements = projectNavigations.Where(s => s.parent_id == 0);
                foreach (CustomProjectNavigation pn in rootElements)
                    newProjectNavigations.AddRange(OrderTreeProjectSns(pn, projectNavigations));

                return newProjectNavigations;
            }
            catch { throw; }
        }

        public async Task<List<CustomStandardNumberingSystem>> GetProjectSnsAsync(int projectId)
        {
            try
            {
                var snsResult = await (from s1 in _db.ProjectStandardNumberingSystems
                                 join s2 in _db.ProjectStandardNumberingSystems on s1.ParentId equals s2.Id into s3
                                 from s2 in s3.DefaultIfEmpty()
                                 where s1.ProjectId == projectId
                                 select new { s1.Id, s1.Code, s1.Description, s1.CreatedBy, s1.CreatedOn, s1.ParentId, s1.UpdatedBy, s1.UpdatedOn, ParentCode = s2.Code, s1.Snsid }).ToListAsync();

                List<CustomStandardNumberingSystem> snSystems = new List<CustomStandardNumberingSystem>();
                foreach (var item in snsResult)
                {
                    snSystems.Add(new CustomStandardNumberingSystem()
                    {
                        Id = item.Snsid.Value, Code = item.Code, Description = item.Description,
                        CreatedBy = item.CreatedBy, CreatedOn = item.CreatedOn, ParentCode = item.ParentCode,
                        ParentId = item.ParentId, UpdatedBy = item.UpdatedBy, UpdatedOn = item.UpdatedOn,
                        SnsId = item.Snsid, parent_id = item.ParentId, title = item.Code + " : " + item.Description
                    });
                }

                AssignLevelProject(snSystems);

                List<CustomStandardNumberingSystem> newStandardNumbers = new List<CustomStandardNumberingSystem>();
                IEnumerable<CustomStandardNumberingSystem> rootElements = snSystems.Where(s => s.parent_id == 0);
                foreach (CustomStandardNumberingSystem sns in rootElements)
                    newStandardNumbers.AddRange(OrderTreeProjectSns(sns, snSystems));

                return newStandardNumbers;
            }
            catch { throw; }
        }

        public async Task<List<LocationCode>> GetLocationCodesAsync(int projectId)
        {
            Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) return new List<LocationCode>();
            return await _db.LocationCodes.Where(l => l.LocationCodeSetId == project.LocationCodeId).ToListAsync();
        }

        public async Task<List<InformationCode>> GetInformationCodesAsync(int projectId)
        {
            Project project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) return new List<InformationCode>();
            return await _db.InformationCodes.Where(i => i.InformationCodeSetId == project.InformationCodeId).ToListAsync();
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            return await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<string> GetSnsCodeAsync(int snsId, int projectId)
        {
            string snsCode = string.Empty;
            int depth = 0;
            ProjectStandardNumberingSystem sns = await _db.ProjectStandardNumberingSystems.FirstOrDefaultAsync(s => s.Snsid == snsId);
            await GetSnsCodeAsync(sns.Snsid.Value, projectId, snsCode, depth, (code, dep) => { snsCode = code; depth = dep; });

            int nodesToAdd = 4 - depth % 4;
            if (nodesToAdd < 4)
            {
                for (int count = 0; count < nodesToAdd; count++)
                    snsCode = snsCode + "00-";
            }
            return snsCode.Substring(0, snsCode.Length - 1);
        }

        private async Task GetSnsCodeAsync(int snsId, int projectId, string currentCode, int currentDepth, Action<string, int> callback)
        {
            ProjectStandardNumberingSystem sns = await _db.ProjectStandardNumberingSystems
                .FirstOrDefaultAsync(s => s.Snsid == snsId && s.ProjectId == projectId);

            if (sns.ParentId > 0)
            {
                string parentCode = string.Empty;
                int parentDepth = 0;
                await GetSnsCodeAsync(sns.ParentId, projectId, parentCode, parentDepth, (code, dep) => { parentCode = code; parentDepth = dep; });
                callback(parentCode + sns.Code + "-", parentDepth + 1);
            }
            else
            {
                callback(currentCode + sns.Code + "-", currentDepth + 1);
            }
        }
    }
}
