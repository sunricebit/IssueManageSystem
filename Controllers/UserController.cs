using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Firebase.Auth;
using Firebase.Storage;
using IMS.Models;
using IMS.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.IO;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace IMS.Controllers
{
    [Route("/User")]
    public class UserController : Controller
    {
        private static string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
        private static string Bucket = "imsmanagement-35781.appspot.com";
        private static string AuthEmail = "abc@gmail.com";
        private static string AuthPassword = "123456";
        private readonly IUserService userService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly IMailService _mailService;
        private readonly IHashService _hashService;
        public UserController(IUserService service, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IMailService mailService, IHashService hashService)
        {
            userService = service;
            _env = env;
            _mailService = mailService;
            _hashService = hashService;
        }
        [HttpGet]
        public IActionResult Index(int? pageNumber)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Models.User> paginate = new Paginate<Models.User>(tempPageNumber, tempPageSize);
            //     ViewBag.CreateSuccessMessage = TempData["CreateSuccessMessage"] as string;
            var users = userService.GetAllUsers();
            ViewBag.UserList = paginate.GetListPaginate<Models.User>(users);
            ViewBag.Action = "UserList";
            var role = userService.GetRole();
            ViewBag.Roles = role;
            ViewBag.Pagination = paginate.GetPagination();


            return View();
        }
        [HttpGet("Search")]
        public IActionResult Search(int? pageNumber, string keyword)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Models.User> paginate = new Paginate<Models.User>(tempPageNumber, tempPageSize);
            var role = userService.GetRole();
            ViewBag.Roles = role;
            ViewBag.Pagination = paginate.GetPagination();
            var users = userService.SearchUsers(keyword);
            ViewBag.UserList = paginate.GetListPaginate<Models.User>(users);
            return View("Index");
        }
        [HttpGet("FilterRole")]
        public IActionResult FilterByRole(int? pageNumber, int roleid)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Models.User> paginate = new Paginate<Models.User>(tempPageNumber, tempPageSize);
            var role = userService.GetRole();
            ViewBag.Roles = role;
            ViewBag.Pagination = paginate.GetPagination();

            var users = userService.FilterByRole(roleid);
            ViewBag.UserList = paginate.GetListPaginate<Models.User>(users);
            return View("Index");

        }
        [HttpGet("FilterStatus")]
        public IActionResult FilterByStatus(int? pageNumber, string status)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Models.User> paginate = new Paginate<Models.User>(tempPageNumber, tempPageSize);
            var role = userService.GetRole();
            ViewBag.Roles = role;
            bool status2;
            ViewBag.Pagination = paginate.GetPagination();
            if (status == "Active")
            {
                status2 = true;
            }
            else
            {
                status2 = false;
            }
            var users = userService.FilterByStatus(status2);
            ViewBag.UserList = paginate.GetListPaginate<Models.User>(users);
            return View("Index");

        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var user = userService.GetUser(id);
            var role = userService.GetRole();
            ViewBag.Roles = role;

            if (user == null)
            {
                return NotFound();
            }
            UserViewModel userViewModel = new UserViewModel()
                {
                Id = user.Id,
                RoleId = user.RoleId,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                Gender = user.Gender,
                Avatar = user.Avatar,
                Status = user.Status

            };

            return View(userViewModel);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var role = userService.GetRole();
            ViewBag.Roles = role;
            return View(new UserViewModel());
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserViewModel? userView, IFormFile avatarFile)
        {

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {

                    avatarFile.CopyTo(fileStream);

                }
                var fileStream2 = new FileStream(filePath, FileMode.Open);
                await Task.Run(() => Upload(fileStream2, avatarFile.FileName));

                userView.Avatar = filePath;
            }

            

            if (userView.Avatar == null)
            {
                userView.Avatar = "https://firebasestorage.googleapis.com/v0/b/imsmanagement-35781.appspot.com/o/User%2Fdefault_avatar.jpg?alt=media&token=c9ec5062-d46b-4009-a04a-4fbeb5532005";
            }
            userView.Status = true;
            userView.Password = _mailService.SendRandomPassword(userView.Email);
            Models.User user = new Models.User()
            {
                RoleId = userView.RoleId,
                Name = userView.Name,
                Email = userView.Email,
                Password = userView.Password,
                Address = userView.Address,
                Gender = userView.Gender,
                Avatar = userView.Avatar,
                Status = userView.Status
            };
            userService.AddUser(user);

            var roles = userService.GetRole();
            ViewBag.Roles = roles;

            return View(user);
        }
        public async void Upload(FileStream stream, string filename)
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


            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception was thrown : {0}", ex);
            }
        }



        [HttpPost("Import")]
        public IActionResult Import(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);


                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);


                    for (int i = 2; i <= worksheet.RowsUsed().Count(); i++)
                    {

                        var id = worksheet.Cell(i, 1).Value.ToString();
                        var email = worksheet.Cell(i, 2).Value.ToString();
                        var name = worksheet.Cell(i, 3).Value.ToString();
                        var Role = worksheet.Cell(i, 4).Value.ToString();
                        var Phone = worksheet.Cell(i, 5).Value.ToString();
                        var Address = worksheet.Cell(i, 6).Value.ToString();
                        var existingUser = userService.GetUserByEmail(email);
                        if (id == "Id") continue;
                        if (existingUser != null)
                        {
                            existingUser.Id = int.Parse(id);
                            existingUser.Name = name;
                            var roleid = userService.GetRoleId(Role);
                            existingUser.RoleId = roleid;
                            existingUser.Phone = Phone;
                            existingUser.Address = Address;

                            userService.UpdateUser(existingUser);
                        }
                        else
                        {
                            var user = new Models.User()
                            {
                                Id = int.Parse(id),
                                Email = email,
                                Name = name,
                                Phone = Phone,
                                Address = Address,
                                RoleId = userService.GetRoleId(Role),
                                Password = _hashService.HashPassword("123456789"),
                                Gender = true
                            };


                            userService.AddUser(user);
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
        [HttpGet("Export")]
        public IActionResult Export()
        {
            var user = userService.GetAllUsers();
            var filename = "user.xlsx";
            return GenerateExcel(filename, user);
        }
        private FileResult GenerateExcel(string filename, IEnumerable<Models.User> users)
        {
            DataTable data = new DataTable("User");
            data.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("Email"),
                new DataColumn("Name"),
                new DataColumn("Role"),
                new DataColumn("Phone"),
                new DataColumn("Address"),
                new DataColumn("Status")
            });
            foreach (var user in users)
            {
                data.Rows.Add(user.Id, user.Email, user.Name, user.Role.Value, user.Phone, user.Address, user.Status);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(data);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(),
                       "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       filename);
                }
            }
        }

        [HttpPost]

        public IActionResult Update(UserViewModel? userView)
        {
         
            Models.User user = userService.GetUser(userView.Id);
            user.Email = userView.Email;
            user.Name = userView.Name;
            user.Phone = userView.Phone;
            user.Address = userView.Address;
            user.Status = userView.Status;
            user.Avatar = userView.Avatar;
          //  user.Gender = userView.Gender;

            userService.UpdateUser(user);
            //   ViewBag.SuccessMessage = "Update user success!";

           
            return RedirectToAction("Index");
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            return RedirectToAction("Index");
        }


    }

}

