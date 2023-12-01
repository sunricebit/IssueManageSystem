using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Setting")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [Route("List")]
        public IActionResult ViewList(int? pageNumber, int? pageSize)
        {
            int tempPageNumber = pageNumber ?? 2;
            int tempPageSize = pageSize ?? 2;
            ViewBag.SettingList = _settingService.GetSettingPaginate(tempPageNumber, tempPageSize);
            return View();
        }
    }
}
