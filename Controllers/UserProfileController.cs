using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    public class UserProfileController : Controller
    {
        [Route("/userprofile")]
        public IActionResult Index()
        {
            using IMSContext db = new IMSContext();
            var User = db.Users.SingleOrDefault(u => u.Id == 1);
            return View(User);
        }

        //public async Task<IActionResult> Details()
        //{
            
        //}
    }
}
