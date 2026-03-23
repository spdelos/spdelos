using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index() => RedirectToAction("Identity", "Login");

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
                return RedirectToAction("Index", "Administration");

            ViewBag.error = result.IsLockedOut ? "Account is locked out." : "Invalid Account";
            return View("Index");
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
