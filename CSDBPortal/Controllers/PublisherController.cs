using CSDBPortal.Data;
using CSDBPortal.Models;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Controllers
{
    public class PublisherController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new PublisherViewModel
            {
                Projects = await _db.Projects
                    .OrderBy(p => p.Name)
                    .ToListAsync(),

                PngLogos = await _db.ImageAssets
                    .Where(i => i.MimeType == "image/png")
                    .OrderBy(i => i.Name)
                    .Select(i => new ImageAsset
                    {
                        Id       = i.Id,
                        Name     = i.Name,
                        FileName = i.FileName,
                        MimeType = i.MimeType
                        // Data intentionally omitted; loaded on demand via /Manage/ViewImageAsset
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        /// <summary>
        /// Publish IETP package. Full implementation to be added when publish logic is provided.
        /// </summary>
        [HttpPost]
        public IActionResult PublishIetp(
            int projectId,
            int? logoId,
            string security,
            string status,
            string htmlSource,
            IFormFile? htmlFile,
            IFormFileCollection? assets)
        {
            // TODO: implement IETP publish logic
            return Json(new { success = false, message = "IETP publishing not yet implemented." });
        }
    }
}
