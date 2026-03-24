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
                    administrationViewModel.Features.Add(field.GetValue(null).ToString());

                // Load quick access items, seeding defaults on first run
                administrationViewModel.QuickAccessItems = await GetOrSeedQuickAccessItemsAsync(activeOnly: true);

                return administrationViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<CSDBPortal.Models.QuickAccessItem>> GetOrSeedQuickAccessItemsAsync(bool activeOnly)
        {
            if (!await _db.QuickAccessItems.AnyAsync())
            {
                var defaults = new List<CSDBPortal.Models.QuickAccessItem>
                {
                    new() { Title="Generate ICN",   Href="ICN",                    IconClass="fa-solid fa-hashtag",         IconColor="#2563eb", SortOrder=1,  IsActive=true  },
                    new() { Title="Manage",          Href="Manage",            IconClass="fa-solid fa-file-circle-plus",IconColor="#16a34a", SortOrder=2,  IsActive=true  },
                    new() { Title="Configure",       Href="Configuration",          IconClass="fa-solid fa-sliders",         IconColor="#7c3aed", SortOrder=3,  IsActive=true  },
                    new() { Title="BREX Rules",      Href="Manage?tab=projects",IconClass="fa-solid fa-code",           IconColor="#d97706", SortOrder=4,  IsActive=true  },
                    new() { Title="Workflow",        Href="Workflow",               IconClass="fa-solid fa-diagram-project", IconColor="#0891b2", SortOrder=5,  IsActive=true  },
                    new() { Title="Task",            Href="Task",                   IconClass="fa-solid fa-list-check",      IconColor="#64748b", SortOrder=6,  IsActive=false },
                    new() { Title="Publisher",       Href="Publisher",              IconClass="fa-solid fa-print",           IconColor="#dc2626", SortOrder=7,  IsActive=false },
                    new() { Title="Viewer",          Href="Viewer",                 IconClass="fa-solid fa-eye",             IconColor="#059669", SortOrder=8,  IsActive=false },
                    new() { Title="Licensing",       Href="Licensing",              IconClass="fa-solid fa-key",             IconColor="#9333ea", SortOrder=9,  IsActive=false },
                    new() { Title="Administration",  Href="Administration",         IconClass="fa-solid fa-chart-pie",       IconColor="#2563eb", SortOrder=10, IsActive=false },
                };
                _db.QuickAccessItems.AddRange(defaults);
                await _db.SaveChangesAsync();
            }

            var query = _db.QuickAccessItems.OrderBy(q => q.SortOrder);
            return activeOnly
                ? await query.Where(q => q.IsActive).ToListAsync()
                : await query.ToListAsync();
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
