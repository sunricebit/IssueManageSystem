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
        public IActionResult ViewList(int? pageNumber)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Setting> paginate = new Paginate<Setting>(_settingService.GetSettings(), tempPageNumber, tempPageSize);
            ViewBag.SettingList = paginate.GetListPaginate();
            ViewBag.Action = "ViewList";
            ViewBag.Pagination = paginate.GetPagination();
            return View();
        }
    }
}
