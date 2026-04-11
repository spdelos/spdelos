using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSDBPortal.Controllers
{
    public class ViewerController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly ManageManager _manageManager;

        public ViewerController(ApplicationDbContext db, ManageManager manageManager)
        {
            _db = db;
            _manageManager = manageManager;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new ViewerViewModel
            {
                CanView  = HasPermission(Features.Viewer),
                Projects = HasPermission(Features.Viewer)
                    ? await _db.Projects.OrderBy(p => p.Name).ToListAsync()
                    : new()
            };
            return View(vm);
        }

        // ── Navigation tree ────────────────────────────────────────────────
        [HttpPost]
        public async Task<JsonResult> GetNavTree(int projectId)
        {
            if (!HasPermission(Features.Viewer))
                return PermissionDenied();

            var tree = await _manageManager.GetProjectNavigationTreeAsync(projectId);
            return Json(tree);
        }

        // ── Module XML content ─────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> GetModule(int id)
        {
            if (!HasPermission(Features.Viewer))
                return PermissionDenied();

            var dmc = await _db.DataModuleCodes
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (dmc == null)
                return NotFound();

            return Json(new
            {
                id      = dmc.Id,
                dmc     = dmc.DMC,
                title   = $"{dmc.TechName} \u2014 {dmc.InfoName}",
                xml     = dmc.xml ?? string.Empty
            });
        }

        // ── Sub-viewer pages (loaded inside the content panel) ────────────

        /// <summary>CGM image viewer — adapted from csdblite viewer.html</summary>
        [HttpGet]
        public IActionResult CgmViewer(int dmcId, string imageId)
        {
            ViewBag.DmcId   = dmcId;
            ViewBag.ImageId = imageId;
            return View();
        }

        /// <summary>Video viewer — adapted from csdblite video-viewer.html</summary>
        [HttpGet]
        public IActionResult VideoViewer(int dmcId, string videoId)
        {
            ViewBag.DmcId   = dmcId;
            ViewBag.VideoId = videoId;
            return View();
        }

        /// <summary>3-D model viewer — adapted from csdblite threed-viewer.html</summary>
        [HttpGet]
        public IActionResult ThreeDViewer(int dmcId, string mediaId)
        {
            ViewBag.DmcId   = dmcId;
            ViewBag.MediaId = mediaId;
            return View();
        }

        // ── Media file delivery ───────────────────────────────────────────
        /// <summary>
        /// Serves a raw media file (CGM, MP4, GLB …) located inside any
        /// published NavPackage on disk.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMedia(int dmcId, string filename)
        {
            if (!HasPermission(Features.Viewer))
                return Forbid();

            var dmc = await _db.DataModuleCodes
                .FirstOrDefaultAsync(d => d.Id == dmcId && !d.IsDeleted);
            if (dmc == null)
                return NotFound();

            var env = HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>();

            var navDir = Path.Combine(env.ContentRootPath, "NavPackages");
            if (Directory.Exists(navDir))
            {
                var found = Directory.EnumerateFiles(navDir, filename,
                        SearchOption.AllDirectories)
                    .FirstOrDefault();

                if (found != null)
                {
                    var mime = Path.GetExtension(filename).ToLowerInvariant() switch
                    {
                        ".mp4" => "video/mp4",
                        ".glb" => "model/gltf-binary",
                        ".cgm" => "image/cgm",
                        ".png" => "image/png",
                        ".jpg" or ".jpeg" => "image/jpeg",
                        _ => "application/octet-stream"
                    };
                    return PhysicalFile(found, mime);
                }
            }

            return NotFound();
        }
    }
}
