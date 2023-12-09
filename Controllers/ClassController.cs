using DocumentFormat.OpenXml.Spreadsheet;
using IMS.Models;
using IMS.Services;
using IMS.ViewModels.Class;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace IMS.Controllers
{
    [Route("/Class")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        public ClassController(IClassService classService, IUserService userService)
        {
            _classService = classService;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Index(int? pageNumber, bool? filterbyStatus, string? searchByValue, string? filterByTeacher)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 5;
            Paginate<Class> paginate = new Paginate<Class>(tempPageNumber, tempPageSize);
            Dictionary<string, dynamic> filter = new Dictionary<string, dynamic>(), search = new Dictionary<string, dynamic>();
            if (filterByTeacher != null && !filterByTeacher.Equals("All"))
            {
                int teacherid = _classService.GetTeacherIdByName(filterByTeacher);
                filter.Add("TeacherId", teacherid);
            }
            if (filterbyStatus != null && !filterbyStatus.Equals("All"))
            {
                filter.Add("Status", filterbyStatus);
            }
          
            if (!string.IsNullOrEmpty(searchByValue))
            {
                search.Add("Name", searchByValue);
                search.Add("Description", searchByValue);
               
            }
           
            ViewBag.TeacherValue = _classService.GetAllTeachers();
            ViewBag.StatusValue = filterbyStatus;
            ViewBag.SearchValue = searchByValue;
            List<Class> classes = new List<Class>();
            foreach(var Class in paginate.GetListPaginate<Class>(filter,search))
            {
                var classWithStudents = _classService.GetClass(Class.Id);
                classes.Add(classWithStudents);

            }
            ViewBag.ClassList = classes;
            ViewBag.Action = "ClassList";
            ViewBag.Pagination = paginate.GetPagination();

            return View();
        }
        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var subject = _classService.GetSubjects();
            ViewBag.Subject = subject;
            var teacher = _userService.GetTeacher();
            ViewBag.Teacher = teacher;
            var Class = _classService.GetClass(id);
            if (Class == null)
            {
                return NotFound();
            }
            ClassViewModel ClassViewModel = new ClassViewModel()
            {
                Id = Class.Id,
                Name = Class.Name,
                Description = Class.Description,
                TeacherId = Class.TeacherId,
                SubjectId = Class.SubjectId,
                Status = Class.Status,
                Teacher = Class.Teacher,
                Subject = Class.Subject,
                Milestones = Class.Milestones,
                IssueSettings = Class.IssueSettings
            };
           
            return View(ClassViewModel);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {

            var subject = _classService.GetSubjects();
            ViewBag.Subject = subject;
            return View();

        }
        [HttpPost("Create")]
        public IActionResult Create(ClassViewModel classViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            classViewModel.Status = true;
            Class Class = new Class()
            {
                Id= classViewModel.Id,
                Description = classViewModel.Description,
                TeacherId = classViewModel.TeacherId,
                SubjectId = classViewModel.SubjectId,
                Status = classViewModel.Status
            };
            _classService.AddClass(Class);

            var subject = _classService.GetSubjects();
            ViewBag.Subject = subject;
            ViewBag.SuccessMessage = "Add class success!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Update(ClassViewModel classViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var subject = _classService.GetSubjects();
            ViewBag.Subject = subject;
            var teacher = _userService.GetTeacher();
            ViewBag.Teacher = teacher;
            Class Class1 = _classService.GetClass(classViewModel.Id);
            Class1.SubjectId = classViewModel.SubjectId;  
           Class1.TeacherId = classViewModel.TeacherId;
            Class1.Name = classViewModel.Name;
            Class1.Status = classViewModel.Status;
            Class1.Description = classViewModel.Description;
            _classService.UpdateCLass(Class1);
            return RedirectToAction("Index");
        }
        [HttpPost("UpdateStatus")]
        public IActionResult ToggleStatus(int id)
        {
            var Class = _classService.GetClass(id);

            if (Class == null)
            {
                return NotFound();
            }

            Class.Status = !Class.Status;

            _classService.UpdateCLass(Class);

            return RedirectToAction("Index");
        }
    }
}
