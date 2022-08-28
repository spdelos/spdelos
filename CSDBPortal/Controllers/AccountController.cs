using CSDBPortal.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CSDBPortal.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Identity", "Login");
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string email, string password)
        {

            using (ApplicationDbContext applicationContext = new())
            {
                if (email.Trim() == "testuser@gmail.com" && password.Trim() == "password")
                {

                    var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, email),
                        new Claim(ClaimTypes.Name, email)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Administration");
                }
                else
                {
                    ViewBag.error = "Invalid Account";
                    return View("Index");
                }
            }
            /* TODO: need to extend the functonality  by asp.net user
            var userdetails = db.UserDetails.Where(x => x.UserId== username && x.UserId == password).FirstOrDefault();
            if (userdetails != null)
            {
                String userid = userdetails.UserId.ToString();


                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(userdetails.UserId)),
                    new Claim(ClaimTypes.Name, userdetails.UserId.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Administration");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }*/
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
