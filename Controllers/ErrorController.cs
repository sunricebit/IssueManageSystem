using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("NotAccess")]
        public IActionResult NotAccess()
        {
            return View();
        }
    }
}
