using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using IMS.ViewModels.Auth;

namespace IMS.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IMSContext _context;
        private static string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
        private static string Bucket = "imsmanagement-35781.appspot.com";
        private static string AuthEmail = "abc@gmail.com";
        private static string AuthPassword = "123456";
        public UserProfileController(IMSContext context)
        {
            _context = context;
        }
        [Route("/userprofile")]
        [CustomAuthorize]
        public IActionResult User([FromQuery] string tab)
        {
            try
            {
                ViewData["tab"] = tab ?? "userdetails";
                switch (tab)
                {
                    case "userdetails":
                        {
                            int? id = HttpContext.Session.GetUser()?.Id;
                            var User = _context.Users.SingleOrDefault(u => u.Id == id);
                            return View("UserDetail", User);
                        }

                   

                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return View();
            }

        }
        public async Task<IActionResult> EditUserProfile()
        {
            try
            {
                int? id = HttpContext.Session.GetUser()?.Id;
                var User = _context.Users.SingleOrDefault(u => u.Id == id);
                return View(User);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile([Bind("Id,Name,Phone,Address,Gender")] Models.User User)
        {
            try
            {
                Models.User user = _context.Users.Find(User.Id);
                user.Name = User.Name;
                user.Phone = User.Phone;
                user.Address = User.Address;
                user.Gender = User.Gender;
                _context.SaveChanges();

                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }


        }


        [HttpPost]
        public async Task<IActionResult> CreateAvatar(IFormFile file)
        {
            try
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


                    var fileStream2 = new FileStream(filePath, FileMode.Open);
                    var downloadLink = await Upload(fileStream2, file.FileName);
                    int? id = HttpContext.Session.GetUser()?.Id;
                    Models.User avatar = _context.Users.SingleOrDefault(u => u.Id == id);
                    avatar.Avatar = downloadLink;
                    _context.SaveChanges();
                    ViewBag.Success = "Create avatar successful.";
                }

                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }

        }
        public async Task<string> Upload(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                ).Child("User")
                 .Child(filename)
                 .PutAsync(stream, cancellation.Token);
            try
            {
                string link = await task;
                return link;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception was thrown : {0}", ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(IFormFile file)
        {
            try
            {
                int? id = HttpContext.Session.GetUser()?.Id;
                // Xử lý cập nhật avatar và lưu vào cơ sở dữ liệu
                var existingAvatar = _context.Users.FirstOrDefault(u => u.Id == id);

                if (existingAvatar.Avatar != null)
                {
                    string[] arr = existingAvatar.Avatar.Split('=');
                    if (file != null && file.Length > 0)
                    {
                        //// Xóa file avatar 
                        //var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars");
                        //// Tạo đối tượng DirectoryInfo
                        //DirectoryInfo directoryInfo = new DirectoryInfo(oldFilePath);

                        //// Lấy danh sách tất cả các tệp tin trong thư mục
                        //FileInfo[] files = directoryInfo.GetFiles();

                        //// Xóa tất cả các tệp tin
                        //foreach (FileInfo fileOfAvatars in files)
                        //{

                        //    fileOfAvatars.Delete();
                        //}

                        // Tạo file avatar mới
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // Cập nhật thông tin avatar trong cơ sở dữ liệu
                        var fileStream2 = new FileStream(filePath, FileMode.Open);
                        var downloadLink = await Upload(fileStream2, file.FileName);
                        existingAvatar.Avatar = downloadLink;
                        _context.SaveChanges();


                        ViewBag.Success = "Update avatar successful.";

                    }
                }

                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }

        }


        [HttpPost]
        public IActionResult DeleteAvatar()
        {
            try
            {
                int? id = HttpContext.Session.GetUser()?.Id;
                // Xử lý xóa avatar và cập nhật cơ sở dữ liệu
                var avatarToDelete = _context.Users.FirstOrDefault(a => a.Id == id);

                if (avatarToDelete.Avatar != null)
                {
                    // Xóa file avatar
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", avatarToDelete.Avatar);
                    //System.IO.File.Delete(filePath);

                    // Xóa avatar trong cơ sở dữ liệu
                    // Cập nhật thông tin avatar trong cơ sở dữ liệu
                    avatarToDelete.Avatar = null;
                    _context.SaveChanges();
                }
                ViewBag.Success = "Delete avatar successful";
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }

        }


        
    }
}
