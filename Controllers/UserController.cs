using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace IMS.Controllers
{
    [Route("/User")]
    public class UserController : Controller
    {
        private static string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
        private static string Bucket = "imsmanagement-35781.appspot.com";
        private static string AuthEmail = "dinhthenam10102@gmail.com";
        private static string AuthPassword = "mid04052002";
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
            Paginate<User> paginate = new Paginate<User>(tempPageNumber, tempPageSize);
            ViewBag.CreateSuccessMessage = TempData["CreateSuccessMessage"] as string;
            var users = userService.GetAllUsers();
            ViewBag.UserList = paginate.GetListPaginate<User>(users);
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
                Paginate<User> paginate = new Paginate<User>(tempPageNumber, tempPageSize);
                var role = userService.GetRole();
                ViewBag.Roles = role;
                ViewBag.Pagination = paginate.GetPagination();
                var users = userService.SearchUsers(keyword);
            ViewBag.UserList = paginate.GetListPaginate<User>(users);
            return View("Index");
        }
        [HttpGet("FilterRole")]
        public IActionResult FilterByRole(int? pageNumber,int roleid)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<User> paginate = new Paginate<User>(tempPageNumber, tempPageSize);
            var role = userService.GetRole();
            ViewBag.Roles = role;
            ViewBag.Pagination = paginate.GetPagination();
           
            var users = userService.FilterByRole(roleid);
            ViewBag.UserList = paginate.GetListPaginate<User>(users);
            return View("Index");

        }
        [HttpGet("FilterStatus")]
        public IActionResult FilterByStatus(int? pageNumber, string status)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<User> paginate = new Paginate<User>(tempPageNumber, tempPageSize);
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
            ViewBag.UserList = paginate.GetListPaginate<User>(users);
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

            return View(user);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var role = userService.GetRole();
            ViewBag.Roles = role; 
            return View(new User());
        }
        [HttpPost("Create")]
        public IActionResult Create(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                ModelState.AddModelError("Email", "Email is required.");
            }
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
            }

            if (!ModelState.IsValid)
            {
                if (user.Avatar == null)
                {
                    user.Avatar = "https://firebasestorage.googleapis.com/v0/b/imsmanagement-35781.appspot.com/o/User%2Fdefault_avatar.jpg?alt=media&token=c9ec5062-d46b-4009-a04a-4fbeb5532005";
                }
                user.Password = _mailService.SendRandomPassword(user.Email);
                userService.AddUser(user);
                TempData["CreateSuccessMessage"] = "Người dùng đã được thêm thành công.";
                return RedirectToAction("Index","User");
            }

           
            var roles = userService.GetRole();
            ViewBag.Roles = roles;

            return View(user);
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
                            var user = new User()
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
        private FileResult GenerateExcel(string filename, IEnumerable<User> users)
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
            foreach ( var user in users)
            {
                data.Rows.Add(user.Id,user.Email,user.Name,user.Role.Value,user.Phone,user.Address,user.Status);
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

        [HttpPost()]
        
        public IActionResult Update(User user)
        {
            if (!ModelState.IsValid)
            {
                userService.UpdateUser(user);
                return RedirectToAction("Index");
            }

            return View("Index");
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            return RedirectToAction("Index");
        }


    }

}

