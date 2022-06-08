using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;

namespace CSDBPortal.Business
{
    public class ICNManager
    {

        public ICNViewModel GetProjects()
        {
            ICNViewModel _iCNViewModel = new ICNViewModel();
            _iCNViewModel.ProjectList  = new List<ProjectDropDown>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from p in _db.Projects
                                  select new { p.Id, p.Name }).ToList();
                    foreach (var obj in result)
                    {
                        _iCNViewModel.ProjectList.Add(new ProjectDropDown { ProjecctId = obj.Id, Name = obj.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return _iCNViewModel;
        }
        public List<CustomICNNumber> ICNNumberByProjectId(int projectid)
        {
            List<CustomICNNumber> customICNNumberList = new List<CustomICNNumber>();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    var result = (from icn in _db.IcnNumbers
                                  join  p in _db.Projects on icn.ProjectId equals p.Id
                                  where icn.ProjectId == projectid
                                  select new { icn.Id,icn.Number,icn.UpdatedBy,icn.ProjectId,p.Name }).ToList();
                    foreach (var obj in result)
                    {
                        customICNNumberList.Add(new CustomICNNumber { Id = obj.Id,
                            Number = obj.Number,
                            UpdatedBy=obj.UpdatedBy,
                            ProjectId=obj.ProjectId,
                            ProjectName=obj.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return customICNNumberList;
        }



    }
    public class CustomICNNumber : IcnNumber
    {
        public string ProjectName { set; get; }
    }
}
