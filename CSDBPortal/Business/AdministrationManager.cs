using CSDBPortal.Data;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CSDBPortal.Business
{
    public class AdministrationManager
    {
        public AdministrationViewModel GetAdministrationDetailInfo()
        {
            AdministrationViewModel administrationViewModel = new();
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    // todo : need to modify the below linq query with proper tables and condition.
                    administrationViewModel.ProjectCompleted = (from i in applicationDbContext.Projects
                                                                 select i).Count();
                    //administrationViewModel.UserCount = (from i in applicationDbContext.UserDetails
                    //                                  select i).Count();
                    //administrationViewModel.ActiveUsers = (from i in applicationDbContext.UserDetails
                    //                                        select i).Count();

                    administrationViewModel.Users = applicationDbContext.Users.ToList();
                    administrationViewModel.Roles = applicationDbContext.Roles.ToList();

                    administrationViewModel.UserCount = administrationViewModel.Users.Count();
                    administrationViewModel.ActiveUsers = administrationViewModel.Users.Count();

                    return administrationViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckDuplicateRole(IdentityRole role)
        {
            int result = 0;
            try
            {
                using (ApplicationDbContext applicationDbContext = new())
                {
                    result = applicationDbContext.Roles.Where(r => r.Name == role.Name).Count();
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
