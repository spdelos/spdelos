using CSDBPortal.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDBPortal.Controllers
{
    public class AdministrationController : BaseController
    {
        private readonly AdministrationManager _administrationManager;
        private readonly BaseManager _baseManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController(
            AdministrationManager administrationManager,
            BaseManager baseManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _administrationManager = administrationManager;
            _baseManager = baseManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _administrationManager.GetAdministrationDetailInfoAsync());
            }
            catch { }
            return View();
        }

        public async Task<JsonResult> GetFeature(string roleId)
        {
            List<string> features = new List<string>();
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (Claim claim in claims)
                    features.Add(claim.Value);
            }
            return Json(features);
        }

        public async Task<JsonResult> CreateRole(IdentityRole role, List<string> features)
        {
            if (string.IsNullOrWhiteSpace(role.Id) || string.Compare(role.Id, "0") == 0)
            {
                var recordCount = await _administrationManager.CheckDuplicateRoleAsync(role);
                if (recordCount > 0) return Json("Duplicate");

                var newRole = new IdentityRole(role.Name);
                var roleCreationResult = await _roleManager.CreateAsync(newRole);
                if (roleCreationResult.Succeeded)
                {
                    foreach (var feature in features)
                        await _roleManager.AddClaimAsync(newRole, new Claim("Permission", feature));
                }
                return Json(roleCreationResult);
            }
            else
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(role.Id);
                identityRole.Name = role.Name;

                var claims = await _roleManager.GetClaimsAsync(identityRole);
                foreach (Claim claim in claims)
                {
                    if (!features.Contains(claim.Value))
                        await _roleManager.RemoveClaimAsync(identityRole, claim);
                }
                foreach (string feature in features)
                {
                    var claim = claims.FirstOrDefault(c => c.Value == feature);
                    if (claim == null)
                        await _roleManager.AddClaimAsync(identityRole, new Claim("Permission", feature));
                }
                return Json(await _roleManager.UpdateAsync(identityRole));
            }
        }

        public async Task<JsonResult> DeleteRole(string id)
        {
            IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
            return Json(await _roleManager.DeleteAsync(identityRole));
        }

        public async Task<JsonResult> GetRoles(string userId)
        {
            IList<string> roles = new List<string>();
            var userFromDB = await _userManager.FindByIdAsync(userId);
            if (userFromDB != null)
                roles = await _userManager.GetRolesAsync(userFromDB);
            return Json(roles);
        }

        public async Task<JsonResult> CreateUser(IdentityUser user, IList<string> roles, bool update)
        {
            IdentityResult result;

            if (!update)
            {
                var userFromDB = await _userManager.FindByNameAsync(user.UserName);
                if (userFromDB != null)
                {
                    result = IdentityResult.Failed(new IdentityError { Code = "Duplicate", Description = "User with same name already exists!" });
                }
                else
                {
                    string password = GetPassword(Request.Headers["Authorization"][0]);
                    result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        foreach (string role in roles)
                            await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }
            else
            {
                var userFromDB = await _userManager.FindByIdAsync(user.Id);
                userFromDB.UserName = user.UserName;
                userFromDB.Email = user.Email;
                userFromDB.PhoneNumber = user.PhoneNumber;
                result = await _userManager.UpdateAsync(userFromDB);

                IList<string> currentRoles = await _userManager.GetRolesAsync(userFromDB);
                foreach (string role in currentRoles)
                {
                    if (!roles.Contains(role))
                        await _userManager.RemoveFromRoleAsync(userFromDB, role);
                }
                foreach (string role in roles)
                {
                    if (!currentRoles.Contains(role))
                        await _userManager.AddToRoleAsync(userFromDB, role);
                }
            }
            return Json(result);
        }

        public async Task<JsonResult> DeleteUser(string id)
        {
            var userFromDB = await _userManager.FindByIdAsync(id);
            return Json(await _userManager.DeleteAsync(userFromDB));
        }

        private string GetPassword(string header)
        {
            string[] content1 = header.Split(' ');
            byte[] decodedBytes = Convert.FromBase64String(content1[1]);
            string decodedTxt = System.Text.Encoding.UTF8.GetString(decodedBytes);
            string[] content2 = decodedTxt.Split(':');
            return content2[1];
        }
    }
}
