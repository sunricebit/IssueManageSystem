using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IMS.Common;


namespace IMS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ICommonService _commonService;

    public HomeController(ILogger<HomeController> logger,IUserService userService)
    {
        _userService = userService;
        _logger = logger;
    }

    [Route("")]
    public IActionResult Index()
    {
        var user = HttpContext.Session.GetUser();
        if (user == null)
        {
            return RedirectToAction("Index", "Landing");
        }
        else
        {
            return RedirectToAction("Blank");
        }
    }

    [Route("BlankDashboard")]
    public IActionResult Blank()
    {
        User user = HttpContext.Session.GetUser();
        ViewBag.LstTimeAccess = user.LstAccessTime == null ? "New account" : user.LstAccessTime.Value.ToString();
        return View("BlankDashboard");
    }
    [Route("UserDashboard")]
    public IActionResult UserDashboard()
    {
        var user = HttpContext.Session.GetUser();
       
        ViewBag.StatusData = _userService.GetPost(user.Id);
        //var categoryPostCounts = _commonService.GetSystemPublishedPostsByCategories();
        //ViewBag.CategoryPostCounts = categoryPostCounts;
        //var authorPostCounts = _commonService.GetSystemPublishedPostsByTopAuthors();
        //ViewBag.AuthorPostCounts = authorPostCounts;

        return View("UserDashboard");
    }

    //test
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

