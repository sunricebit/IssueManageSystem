using IMS.Services;
using IMS.ViewModels.Permission;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IMS.Controllers
{
    [Route("Permission")]
    public class PermissionController : Controller
    {
        private readonly PermissionDAO _permissionDAO;

        public PermissionController(PermissionDAO permissionDAO)
        {
            _permissionDAO = permissionDAO;
        }

        [Route("Manage")]
        [CustomAuthorize]
        public IActionResult PermissionManage()
        {
            List<PermissionViewModel> permissionList = _permissionDAO.GetAllPermission();
            List<string> roles = new List<string>();
            foreach(var permission in permissionList)
            {
                roles.Add(permission.Role);
            }
            ViewBag.Roles = roles;
            return View(permissionList);
        }

        [Route("Update"), HttpPost]
        [CustomAuthorize]
        public IActionResult UpdatePermission([FromBody]List<PermissionViewModel> permissionViewModels, [FromServices] IPermissionService permissionService)
        {
            _permissionDAO.UpdatePermission(permissionViewModels);
            // Get permission of user
            User user = HttpContext.Session.GetUser();
            PermissionViewModel permissionVM = permissionService.GetPermissionViewModel(user.RoleId);
            string permissionString = JsonSerializer.Serialize(permissionVM);
            HttpContext.Session.SetString("Permission", permissionString);
            return RedirectToAction("PermissionManage");
        }
    }
}
