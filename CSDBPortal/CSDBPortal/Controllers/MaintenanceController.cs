using CSDBPortal.Business;
using CSDBPortal.Data;
using CSDBPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSDBPortal.Controllers
{
    public class MaintenanceController : BaseController
    {
        BaseManager _baseManager = new();
        MaintenanceManager maintenancesManager = new();
        //[Authorize]
        public IActionResult Index()
        {
            try
            {
                MaintenanceManager _maintenanceManager = new();
                return View(_maintenanceManager.GetMaintenanceDetailInfo());
            }
            catch (Exception ex)
            {
                //todo
            }
            // todo; need to redirect error page or message
            return View();
        }

        public JsonResult CreateLocationCode(LocationCode locationCode)
        {
            // need to assign login user email here
            locationCode.CreatedBy = User.Identity.Name;
            locationCode.UpdatedOn = DateTime.UtcNow;
            string mode = string.Empty;
            if (locationCode.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckLocationCode(locationCode);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(locationCode, mode));
        }

        public JsonResult DeleteLocationCode(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.LocationCodes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }

        public JsonResult CreateLocationCodeSet(LocationCodeSet locationCodeSet)
        {
            // need to assign login user email here
            locationCodeSet.CreatedBy = User.Identity.Name;
            locationCodeSet.UpdatedOn = DateTime.UtcNow;
            string mode = string.Empty;
            if (locationCodeSet.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckLocationCodeSet(locationCodeSet);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(locationCodeSet, mode));
        }

        public JsonResult DeleteLocationCodeSet(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.LocationCodeSets.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }

        public JsonResult CreateRPC(ResponsiblePartnerCode rpc)
        {
            // need to assign login user email here
          
            string mode = string.Empty;
            if (rpc.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckRPC(rpc);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(rpc, mode));
        }

        public JsonResult DeleteRPC(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var rpc = applicationContext.ResponsiblePartnerCodes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(rpc, ""));
            }
        }



        public JsonResult CreateDataTypeModule(DataModuleType dataModuleType)
        {
            // need to assign login user email here
            dataModuleType.CreatedBy = User.Identity.Name;
            string mode = string.Empty;
            if (dataModuleType.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckDataModuleType(dataModuleType);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(dataModuleType, mode));
        }

        public JsonResult DeleteDataTypeModule(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var dataModuleType = applicationContext.DataModuleTypes.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(dataModuleType, ""));
            }
        }

        public JsonResult CreateProject(Project project)
        {
            project.CreatedBy = User.Identity.Name;

            string mode = string.Empty;
            if (project.Id > 0)
            {
                mode = "Edit";
                project.ModifiedBy = User.Identity.Name;
                project.ModifiedDate = DateTime.UtcNow;
            }
            else
            {
                mode = "Add";
                project.CreatedBy = User.Identity.Name;
                project.CreatedDate = DateTime.UtcNow;
            }

            var recordCount = maintenancesManager.CheckProject(project);
            if (recordCount > 0)
            {
                return Json("Duplicate");
            }
            return Json(_baseManager.CreateOrUpdateRecord(project, mode));
        }

        public JsonResult DeleteProject(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var project = applicationContext.Projects.Where(i => i.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(project, ""));
            }
        }

        public JsonResult CreateSns(StandardNumberingSystem sns)
        {
            // need to assign login user email here
            sns.CreatedBy = User.Identity.Name;
            sns.CreatedOn = DateTime.UtcNow;
            
            string mode = string.Empty;
            if (sns.Id > 0)
            {
                mode = "Edit";
            }
            else
            {
                var recordCount = maintenancesManager.CheckSns(sns);
                if (recordCount > 0)
                {
                    return Json("Duplicate");
                }
                mode = "Add";
            }
            return Json(_baseManager.CreateOrUpdateRecord(sns, mode));
        }

        public JsonResult DeleteSns(int id)
        {
            using (ApplicationDbContext applicationContext = new())
            {
                var locationo = applicationContext.StandardNumberingSystems.Where(s => s.Id == id).FirstOrDefault();
                return Json(_baseManager.DeleteRecord(locationo, ""));
            }
        }
    }
}
