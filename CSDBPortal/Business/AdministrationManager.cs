using CSDBPortal.Data;
using CSDBPortal.Services;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CSDBPortal.Business
{
    public class AdministrationManager
    {
        private readonly ApplicationDbContext _db;
        private readonly ActiveSessionTracker _sessionTracker;

        public AdministrationManager(ApplicationDbContext db, ActiveSessionTracker sessionTracker)
        {
            _db = db;
            _sessionTracker = sessionTracker;
        }

        public async Task<AdministrationViewModel> GetAdministrationDetailInfoAsync()
        {
            AdministrationViewModel administrationViewModel = new();
            try
            {
                administrationViewModel.ProjectCompleted = await _db.Projects.CountAsync();
                administrationViewModel.Users = await _db.Users.ToListAsync();
                administrationViewModel.Roles = await _db.Roles.ToListAsync();
                administrationViewModel.UserCount = administrationViewModel.Users.Count;
                administrationViewModel.ActiveUsers = _sessionTracker.GetActiveCount();

                administrationViewModel.Features = new List<string>();
                var fields = typeof(Features).GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach (FieldInfo field in fields)
                {
                    administrationViewModel.Features.Add(field.GetValue(null).ToString());
                }

                return administrationViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> CheckDuplicateRoleAsync(IdentityRole role)
        {
            try
            {
                return await _db.Roles.CountAsync(r => r.Name == role.Name);
            }
            catch
            {
                return 0;
            }
        }
    }
}
