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

        [Route("404")]
        public IActionResult NotFound()
        {
            return View();
        }

        [Route("InternalServerError")]
        public IActionResult InternalServerError(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
