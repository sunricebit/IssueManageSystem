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
            Paginate<Setting> paginate = new Paginate<Setting>(tempPageNumber, tempPageSize); 
            // Paginate<T> paginate = new Paginate<T>(tempPageNumber, tempPageSize);
            ViewBag.SettingList = paginate.GetListPaginate<Setting>();
            // ViewBag.SettingList = paginate.GetListPaginate<T>();
            ViewBag.Action = "ViewList";
            ViewBag.Pagination = paginate.GetPagination();
            return View();
        }
    }
}
