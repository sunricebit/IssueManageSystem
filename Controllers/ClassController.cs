using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.Models;
using IMS.Services;
using IMS.ViewModels.Class;
using IMS.ViewModels.Milestone;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace IMS.Controllers
{
    [Route("/Class")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;
        private readonly IUserService _userService;
        private readonly IMilestoneService _milestoneService;
        public ClassController(IClassService classService, IUserService userService, IMilestoneService milestoneService)
        {
            _classService = classService;
            _userService = userService;
            _milestoneService = milestoneService;
        }
        [HttpGet]
        public IActionResult Index(int? pageNumber, bool? filterbyStatus, string? searchByValue, string? filterByTeacher, string? filterBySubject)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 5;
            Paginate<Class> paginate = new Paginate<Class>(tempPageNumber, tempPageSize);
            Dictionary<string, dynamic> filter = new Dictionary<string, dynamic>(), search = new Dictionary<string, dynamic>();
            if (filterByTeacher != null && !filterByTeacher.Equals("All"))
            {
                int teacherid = _classService.GetTeacherIdByNameAndEmail(filterByTeacher);
                filter.Add("TeacherId", teacherid);
            }
            if (filterbyStatus != null && !filterbyStatus.Equals("All"))
            {
                filter.Add("IsActive", filterbyStatus);
            }
                if (filterBySubject != null && !filterBySubject.Equals("All"))
                {
                int subjectid = _classService.GetSubjectId(filterBySubject);
                    filter.Add("SubjectId", subjectid);
                }

                if (!string.IsNullOrEmpty(searchByValue))
                {
                    search.Add("Name", searchByValue);
                    search.Add("Description", searchByValue);

                }

                ViewBag.TeacherValue = _classService.GetAllTeachers();
                ViewBag.StatusValue = filterbyStatus;
                ViewBag.SubjectValue = _classService.GetSubjects();
                ViewBag.SearchValue = searchByValue;
                List<Class> classes = new List<Class>();
                foreach (var Class in paginate.GetListPaginate<Class>(filter, search))
                {
                    var classWithStudents = _classService.GetClass(Class.Id);
                    classes.Add(classWithStudents);

                }
                var subject = _classService.GetSubjects();
                ViewBag.Subject = subject;
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
                IsActive = Class.IsActive,
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
        public IActionResult Create(ClassViewModel classViewModel, string teacherInput)
        {
            

            if (!ModelState.IsValid)
            {
                var subject2 = _classService.GetSubjects();
                ViewBag.Subject = subject2;
                return View();
            }
            bool ClassExist = _classService.ClassExist(classViewModel.Name);
            if(ClassExist) 
            {
                var subject4 = _classService.GetSubjects();
                ViewBag.Subject = subject4;
                ViewBag.ErrorMessage = " Class is already exist.";
                return View();
            }
            var teacherid = _classService.GetTeacherIdByNameAndEmail(teacherInput);
            if (teacherid == 0)
            {
                var subject3 = _classService.GetSubjects();
                ViewBag.Subject = subject3;
                ViewBag.ErrorMessage = " Teacher is not exist.";
                return View();
            }

            classViewModel.IsActive = true;
            Class Class = new Class()
            {
                Name = classViewModel.Name,
                Description = classViewModel.Description,
                TeacherId = teacherid,
                SubjectId = classViewModel.SubjectId,
                IsActive = classViewModel.IsActive
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
            Class1.IsActive = classViewModel.IsActive;
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

            Class.IsActive = !Class.IsActive;

            _classService.UpdateCLass(Class);

            return RedirectToAction("Index");
        }
        [HttpGet("People/{id}")]
        public IActionResult People(int id,int page = 1, int pageSize = 5, string searchTerm = "", string filterCat = "", string filterAuthor = "")
        {

            
            
            Dictionary<string, dynamic> filter = new Dictionary<string, dynamic>(), search = new Dictionary<string, dynamic>();

            Class Class = _classService.GetClass(id);
            ClassViewModel ClassViewModel = new ClassViewModel()
            {
                Id = Class.Id,
                Name = Class.Name,
                Description = Class.Description,
                TeacherId = Class.TeacherId,
                SubjectId = Class.SubjectId,
                IsActive = Class.IsActive,
                Teacher = Class.Teacher,
                Subject = Class.Subject,
                Milestones = Class.Milestones,
                IssueSettings = Class.IssueSettings
            };
            User teacher = _classService.GetTeacherById(Class.TeacherId?? 0);
            
            IEnumerable<User> students = _classService.GetStudent(id);
            
            ViewBag.Teacher = teacher;
            
            ViewBag.Student = students;
            ViewBag.Action = "People";
           
            Pagination(page, pageSize, students, searchTerm);

            return View("People",ClassViewModel);
        }
        [HttpGet("Milestones/{id}")]
        public IActionResult Milestones(int id)
        {
            var milestone = _classService.GetMilestone(id);
            ViewBag.MilestoneList = milestone;
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
                IsActive = Class.IsActive,
                Teacher = Class.Teacher,
                Subject = Class.Subject,
                Milestones = Class.Milestones,
                IssueSettings = Class.IssueSettings
            };
            return View("Milestones",ClassViewModel);
        }
        [HttpGet("IssueSetting/{id}")]
        public IActionResult IssueSetting(int id)
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
                IsActive = Class.IsActive,
                Teacher = Class.Teacher,
                Subject = Class.Subject,
                Milestones = Class.Milestones,
                IssueSettings = Class.IssueSettings
            };
            return View("IssueSetting", ClassViewModel);
        }
        [HttpPost("CreateMilestone")]
        public IActionResult CreateMilestone(MilestoneViewModel model)
        {
            Milestone milestone = new Milestone()
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ClassId = model.ClassId,
                ProjectId = model.ProjectId
            };
            _milestoneService.AddMilestone(milestone);

            
            return RedirectToAction("Index");
        }
        [HttpPost("AddStudent")]
        public IActionResult AddStudent(int ClassId, string Name)
        {
            _classService.AddStudentToClass(ClassId, Name);
            return RedirectToAction("Index");
        }
        [HttpPost("People/{id}")]
        public IActionResult RemoveStudent(int id,string email)
        {
            Class Class = _classService.GetClass(id);
            ClassViewModel ClassViewModel = new ClassViewModel()
            {
                Id = Class.Id,
                Name = Class.Name,
                Description = Class.Description,
                TeacherId = Class.TeacherId,
                SubjectId = Class.SubjectId,
                IsActive = Class.IsActive,
                Teacher = Class.Teacher,
                Subject = Class.Subject,
                Milestones = Class.Milestones,
                IssueSettings = Class.IssueSettings
            };
            User teacher = _classService.GetTeacherById(Class.TeacherId ?? 0);

            IEnumerable<User> students = _classService.GetStudent(id);

            ViewBag.Teacher = teacher;

            ViewBag.Student = students;
            _classService.RemoveStudentFromClass(id, email);
            return View("People",ClassViewModel);
        }

        public void Pagination(int page, int pageSize, IEnumerable<User> UserList, string searchTerm)
        {

            
            var totalItems = UserList.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var itemsOnPage = UserList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = searchTerm;
            ViewBag.PostList = itemsOnPage;
        }
        
    }
}
