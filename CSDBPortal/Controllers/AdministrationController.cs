using CSDBPortal.Business;
using CSDBPortal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class AdministrationController : BaseController
    {
        BaseManager baseManager = new();
        AdministrationManager administrationManager = new();
        IServiceProvider serviceProvider;
        IPasswordHasher<IdentityUser> passwordHasher;
        UserManager<IdentityUser> userManager;

        public AdministrationController(IServiceProvider serviceProvider, IPasswordHasher<IdentityUser> passwordHasher)
        {
            this.serviceProvider = serviceProvider;
            this.passwordHasher = passwordHasher;
            userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        public IActionResult Index()
        {
            try
            {
                AdministrationManager _administrationManager = new();
                return View(_administrationManager.GetAdministrationDetailInfo());
            }
            catch (Exception ex)
            {
                //todo
            }
            // todo; need to redirect error page or message
            return View();
        }

        public JsonResult CreateRole(IdentityRole role)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (string.IsNullOrWhiteSpace(role.Id) || string.Compare(role.Id, "0") == 0)
            {
                var recordCount = administrationManager.CheckDuplicateRole(role);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }    
                return Json(roleManager.CreateAsync(new IdentityRole(role.Name)).Result);
            }
            else
            {
                IdentityRole identityRole = roleManager.Roles.Where(r => r.Id == role.Id).FirstOrDefault();
                identityRole.Name = role.Name;
                return Json(roleManager.UpdateAsync(identityRole).Result);
            }
        }

        public JsonResult DeleteRole(string id)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            IdentityRole identityRole = roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();
            return Json(roleManager.DeleteAsync(identityRole).Result);
        }

        private string GetPassword(string header)
        {
            string[] content1 = header.Split(' ');
            byte[] decodedBytes = Convert.FromBase64String(content1[1]);
            string decodedTxt = System.Text.Encoding.UTF8.GetString(decodedBytes);
            string[] content2 = decodedTxt.Split(':');
            return content2[1];
        }

        public JsonResult GetRoles(string userId)
        {
            IList<string> roles = new List<string>();
            var userFromDB = userManager.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (userFromDB != null)
            {
                roles = userManager.GetRolesAsync(userFromDB).Result;
            }

            return Json(roles);
        }

        public JsonResult CreateUser(IdentityUser user, IList<string> roles, bool update)
        {
            IdentityResult result;

            if (update == false)
            {
                var userFromDB = userManager.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();

                if (userFromDB != null)
                {
                    result = IdentityResult.Failed(new IdentityError() { Code = "Duplicate", Description = "User with same name already exists!" });
                }
                else
                {
                    string password = GetPassword(Request.Headers["Authorization"][0]);
                    result = userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        foreach (string role in roles)
                        {
                            IdentityResult roleResult = userManager.AddToRoleAsync(user, role).Result;
                        }
                    }
                }
            }
            else
            {
                var userFromDB = userManager.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                userFromDB.UserName = user.UserName;
                userFromDB.Email = user.Email;
                userFromDB.PhoneNumber = user.PhoneNumber;

                result = userManager.UpdateAsync(userFromDB).Result;

                IList<string> currentRoles = userManager.GetRolesAsync(userFromDB).Result;
                foreach(string role in currentRoles)
                {
                    if (roles.Contains(role) == false)
                    {
                        IdentityResult roleResult = userManager.RemoveFromRoleAsync(userFromDB, role).Result;
                    }
                }

                foreach(string role in roles)
                {
                    if (currentRoles.Contains(role) == false)
                    {
                        IdentityResult roleResult = userManager.AddToRoleAsync(userFromDB, role).Result;
                    }
                }
            }

            return Json(result);
        }

        public JsonResult DeleteUser(string id)
        {
            var userFromDB = userManager.Users.Where(u => u.Id == id).FirstOrDefault();

            return Json(userManager.DeleteAsync(userFromDB).Result);
        }
    }
}
