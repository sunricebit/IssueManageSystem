using IMS.DAO;
using IMS.ViewModels.Permission;
using Microsoft.AspNetCore.Mvc;

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
            foreach(var roleAccess in permissionList.First().RolesAcess)
            {
                roles.Add(roleAccess.Key);
            }
            ViewBag.Roles = roles;
            return View(permissionList);
        }

        [Route("Update"), HttpPost]
        [CustomAuthorize]
        public IActionResult UpdatePermission([FromBody]List<PermissionViewModel> permissionViewModels)
        {
            _permissionDAO.UpdatePermission(permissionViewModels);
            ViewBag.SuccessMessage = "Update Successful";
            return RedirectToAction("PermissionManage");
        }
    }
}
