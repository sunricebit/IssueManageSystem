using ClosedXML.Excel;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;

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
           
            ViewBag.UserList = paginate.GetListPaginate<User>();
            ViewBag.Action = "UserList";
            var role = userService.GetRole();
            ViewBag.Roles = role;
            ViewBag.Pagination = paginate.GetPagination();

            var users = userService.GetAllUsers();
            return View(users);
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

                user.Password = _mailService.SendRandomPassword(user.Email);
                userService.AddUser(user);
                TempData["Message"] = "User created successfully";
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
                        var name = worksheet.Cell(i, 2).Value.ToString();
                        var Role = worksheet.Cell(i, 2).Value.ToString();
                        var Phone = worksheet.Cell(i, 2).Value.ToString();
                        var Address = worksheet.Cell(i, 2).Value.ToString();
                        
                        if (id == "Id") continue;

                        var user = new User()
                        {
                            Id = int.Parse(id),
                            Name = name,
                            Phone = Phone,
                            Address = Address
                        };

                        userService.AddUser(user);
                    }
                }
            }

            return Redirect("Index");
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
                new DataColumn("Name"),
                new DataColumn("Role"),
                new DataColumn("Phone"),
                new DataColumn("Address"),
                new DataColumn("Status")
            });
            foreach ( var user in users)
            {
                data.Rows.Add(user.Id,user.Name,user.Role.Value,user.Phone,user.Address,user.Status);
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

     

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                userService.UpdateUser(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [HttpGet("search")]
        public IActionResult Search(string term)
        {
            var users = userService.SearchUsers(term);
            return View(users);
        }

        [HttpGet("filter")]
        public IActionResult Filter(Dictionary<string, object> filters)
        {
            var users = userService.FilterUsers(filters);
            return View(users);
        }
    }

}

