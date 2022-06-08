using CSDBPortal.Data;
using CSDBPortal.ViewModels;

namespace CSDBPortal.Business
{
    public class AdministrationManager
    {
        public AdministrationViewModel GetAdministrationDetailInfo()
        {
            AdministrationViewModel _administrationViewModel = new();
            try
            {
                using (ApplicationDbContext _db = new())
                {
                    // todo : need to modify the below linq query with proper tables and condition.
                    _administrationViewModel.ProjectCompleted = (from i in _db.Projects
                                                                 select i).Count();
                    _administrationViewModel.Users = (from i in _db.UserDetails
                                                      select i).Count();
                    _administrationViewModel.ActiveUsers = (from i in _db.UserDetails
                                                            select i).Count();
                    return _administrationViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
