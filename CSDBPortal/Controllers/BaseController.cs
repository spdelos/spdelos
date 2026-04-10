using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        /// <summary>Returns true when the signed-in user holds the given permission claim.</summary>
        protected bool HasPermission(string feature)
            => User.HasClaim("Permission", feature);

        /// <summary>Returns true when the signed-in user holds at least one of the given permission claims.</summary>
        protected bool HasAnyPermission(params string[] features)
            => features.Any(f => User.HasClaim("Permission", f));

        /// <summary>
        /// Returns a standardised JSON forbidden response.
        /// Use this in API/AJAX actions when the user lacks a required permission.
        /// </summary>
        protected JsonResult PermissionDenied(string? message = null)
            => Json(new
            {
                success = false,
                message = message ?? "You do not have permission to perform this action."
            });
    }
}
