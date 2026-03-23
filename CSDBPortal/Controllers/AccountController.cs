using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDBPortal.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index() => RedirectToAction("Identity", "Login");

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {
            if (email?.Trim() == "testuser@gmail.com" && password?.Trim() == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Name, email)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Administration");
            }

            ViewBag.error = "Invalid Account";
            return View("Index");
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
