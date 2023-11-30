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

            User user = _context.Users.Find(User.Id);
            user.Name = User.Name;
            user.Phone = User.Phone;
            user.Address = User.Address;
            user.Gender = User.Gender;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index),new {tab= "userdetails" });
        }


        [HttpPost]
        public IActionResult CreateAvatar( IFormFile file)
        {
            // Xử lý tạo avatar và lưu vào cơ sở dữ liệu
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                int? id = HttpContext.Session.GetUser()?.Id;
                User avatar = _context.Users.SingleOrDefault(u=>u.Id == id );
                avatar.Avatar = fileName;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index), new { tab = "userdetails" });
        }

        [HttpPost]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            int? id = HttpContext.Session.GetUser()?.Id;
            // Xử lý cập nhật avatar và lưu vào cơ sở dữ liệu
            var existingAvatar = _context.Users.FirstOrDefault(u => u.Id == id);

            if (existingAvatar.Avatar != null)
            {
                if (file != null && file.Length > 0)
                {
                    // Xóa file avatar cũ
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", existingAvatar.Avatar);
                    System.IO.File.Delete(oldFilePath);

                    // Tạo file avatar mới
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Cập nhật thông tin avatar trong cơ sở dữ liệu
                    existingAvatar.Avatar = fileName;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index), new { tab = "userdetails" });
        }

        [HttpPost]
        public IActionResult DeleteAvatar()
        {
            int? id = HttpContext.Session.GetUser()?.Id;
            // Xử lý xóa avatar và cập nhật cơ sở dữ liệu
            var avatarToDelete = _context.Users.FirstOrDefault(a => a.Id == id );

            if (avatarToDelete.Avatar != null)
            {
                // Xóa file avatar
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", avatarToDelete.Avatar);
                System.IO.File.Delete(filePath);

                // Xóa avatar trong cơ sở dữ liệu
                // Cập nhật thông tin avatar trong cơ sở dữ liệu
                avatarToDelete.Avatar = null;
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index), new { tab = "userdetails" });
        }


    }
}
