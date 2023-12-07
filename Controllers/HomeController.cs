using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IMS.Common;


namespace IMS.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("")]
    public IActionResult Index()
    {
        var user = HttpContext.Session.GetUser();
        if (user == null)
        {
            return RedirectToAction("SignIn", "Auth");
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

