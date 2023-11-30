using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Setting")]
    public class SettingController : Controller
    {
        private readonly IMSContext _context;
        public SettingController(IMSContext context)
        {
            _context = context;
        }

        [Route("List")]
        public IActionResult ViewList()
        {
            ViewBag.SettingList = _context.Settings.ToList();
            return View();
        }
    }
}
