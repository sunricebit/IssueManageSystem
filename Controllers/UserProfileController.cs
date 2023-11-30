using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace IMS.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IMSContext _context;


        public UserProfileController(IMSContext context)
        {
            _context = context;
        }
        [Route("/userprofile")]
        [CustomAuthorize()]
        public IActionResult Index([FromQuery] string tab)
        {
           ViewData["tab"] = tab ?? "userdetails";
            switch (tab)
            {
                case "userdetails":
                    {
                        int? id = HttpContext.Session.GetUser()?.Id;
                        var User = _context.Users.SingleOrDefault(u => u.Id == id);
                        return View(User);
                    }
               
            }
            return View();
        }
        [CustomAuthorize()]
        public async Task<IActionResult> EditUserProfile()
        {
            int? id = HttpContext.Session.GetUser()?.Id;
            var User = _context.Users.SingleOrDefault(u => u.Id == id);
            return View(User);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile( [Bind("Id,Name,Phone,Address,Gender")]User User)
        {

            //_context.Entry<User>(User).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //    _context.SaveChanges();
            User user = _context.Users.Find(User.Id);
            user.Name = User.Name;
            user.Phone = User.Phone;
            user.Address = User.Address;
            user.Gender = User.Gender;
            Console.WriteLine(User.Gender);
            //user.Name = User.Name;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index),new {tab= "userdetails" });
        }
        }
}
