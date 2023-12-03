using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Permission")]
    public class PermissionController : Controller
    {
        [Route("Manage")]
        public IActionResult ManagePermission()
        {
            return View();
        }
    }
}
