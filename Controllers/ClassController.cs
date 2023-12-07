using DocumentFormat.OpenXml.Spreadsheet;
using IMS.Models;
using IMS.Services;
using IMS.ViewModels.Class;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("/Class")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        [HttpGet]
        public IActionResult Index(int? pageNumber, bool? filterbyStatus, string? searchByValue, string? filterByTeacher)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 5;
            Paginate<Class> paginate = new Paginate<Class>(tempPageNumber, tempPageSize);
            Dictionary<string, dynamic> filter = new Dictionary<string, dynamic>(), search = new Dictionary<string, dynamic>();
            if (filterbyStatus != null && !filterbyStatus.Equals("All"))
            {
                filter.Add("Status", filterbyStatus);
            }
            if (!string.IsNullOrEmpty(filterByTeacher) && !filterByTeacher.Equals("ALL"))
            {
                filter.Add("RoleId", Int32.Parse(filterByTeacher));
            }
            if (!string.IsNullOrEmpty(searchByValue))
            {
                search.Add("Name", searchByValue);
                search.Add("Description", searchByValue);
               
            }
            ViewBag.TeacherValue = filterByTeacher;
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
                Status = Class.Status
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
                return View();
            }
            var subject = _classService.GetSubjects();
            ViewBag.Subject = subject;
            Class Class = _classService.GetClass(classViewModel.Id);
        //    Class.SubjectId = classViewModel.SubjectId;
            
         //   Class.TeacherId = classViewModel.TeacherId;
            Class.Name = classViewModel.Name;
            Class.Status = classViewModel.Status;
            _classService.UpdateCLass(Class);
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
