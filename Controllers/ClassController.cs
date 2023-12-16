using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.Models;
using IMS.Services;
using IMS.ViewModels.Class;
using IMS.ViewModels.Milestone;
using IMS.ViewModels.Validation;
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
        private readonly IssueSettingDAO _isDAO;
        public ClassController(IClassService classService, IUserService userService, IMilestoneService milestoneService, IssueSettingDAO isDAO)
        {
            _classService = classService;
            _userService = userService;
            _milestoneService = milestoneService;
            _isDAO = isDAO;
        }
        [HttpGet, Route("Index")]
        [CustomAuthorize]
        public IActionResult Index(int? pageNumber, bool? filterbyStatus, string? searchByValue, string? filterByTeacher, string? filterBySubject, [FromServices] IChkPgAcessService chkPgAcess)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
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
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
            return View();
            
        }

        [HttpGet("Details/{id}")]
        [CustomAuthorize]
        public IActionResult Details(int id, [FromServices] IChkPgAcessService chkPgAcess)
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
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
            return View(ClassViewModel);
        }

        [HttpGet("Create")]
        [CustomAuthorize]
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
            var assignments = _classService.GetAssignments(Class.SubjectId??0);
            foreach (var assignment in assignments)
            {
                var milestone = new Milestone
                {
                    Title = $"Initial Milestone for Class {Class.Name}",
                    Description = "Description for the initial milestone",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    ClassId = Class.Id,
                    AssignmentId = assignment.Id,
                    Status = true
                };
                _milestoneService.AddMilestone(milestone);
            }
        

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
            bool ClassExist = _classService.ClassExist(classViewModel.Name);
            if(ClassExist && Class1.Name!=classViewModel.Name)
            {
                var subject2 = _classService.GetSubjects();
                ViewBag.Subject = subject2;
                var teacher2 = _userService.GetTeacher();
                ViewBag.Teacher = teacher2;
                ViewBag.ErrorMessage = "Class is already exist.";
                return View("Details",classViewModel);
                
            }
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
        [CustomAuthorize]
        public IActionResult People(int id, string searchString, string filterbyStatus, [FromServices] IChkPgAcessService chkPgAcess)
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
            User teacher = _classService.GetTeacherById(Class.TeacherId?? 0);
            
            IEnumerable<User> students = _classService.GetStudent(id);
            ViewBag.Teacher = teacher;
            ViewBag.Student = students;
            ViewBag.Action = "People";
            if (!string.IsNullOrEmpty(filterbyStatus) && filterbyStatus != "ALL")
            {
                if (filterbyStatus == "true")
                {
                    students = students.Where(item => item.Status == true).ToList();
                }
                else
                {
                    students = students.Where(item => item.Status == false).ToList();
                }
            }
            if (!string.IsNullOrEmpty(searchString))
            {


                students = students.Where(item => item.Name.ToLower().Contains(searchString.ToLower())
                || item.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }
            
            ViewBag.Student = students;
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
            return View("People",ClassViewModel);
        }
        [HttpGet("Milestones/{id}")]
        [CustomAuthorize]
        public IActionResult Milestones(int id, string searchString, [FromServices] IChkPgAcessService chkPgAcess)
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
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);

            IEnumerable<Milestone> Milestone = _classService.GetMilestone(id);
            if (!string.IsNullOrEmpty(searchString))
            {
                Milestone = Milestone.Where(item => item.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }
            ViewBag.MilestoneList = Milestone;
            return View("Milestones",ClassViewModel);
        }

        [HttpGet("IssueSetting/{id}")]
        [CustomAuthorize]
        public IActionResult IssueSetting(int id,string searchString, [FromServices] IChkPgAcessService chkPgAcess)
        {
            var Class = _classService.GetClass(id);
            ViewBag.ClassId = id;
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
            List<IssueSetting> issueSettings = _isDAO.GetIssueSettingClass(id);
            if (!string.IsNullOrEmpty(searchString))
            {
                issueSettings = issueSettings.Where(item => item.Type.ToLower().Contains(searchString.ToLower())
                || item.Value.ToLower().Contains(searchString.ToLower())).ToList();
            }
            ViewBag.IssueSettingList = issueSettings;
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
            ViewBag.ClassId = id;

            return View("IssueSetting");
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
                ClassId = model.ClassId
            };
            milestone.Status = false;
            _milestoneService.AddMilestone(milestone);

            
            return RedirectToAction("Index");
        }
        [HttpPost("AddStudent")]
        public IActionResult AddStudent(int ClassId, string Name)
        {
           if (_classService.AddStudentToClass(ClassId, Name) == false)
            {
                ViewBag.ErrorMessage = "Student is already exist.";
            };
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
        [HttpPost("CreateIssueSetting")]
        public IActionResult CreateIssueSetting(IssueSettingViewModel issueSettingViewModel, [FromServices] ErrorHelper message)
        {
            Class @class = _classService.GetClass(issueSettingViewModel.ClassId ?? 0);
            IssueSetting iS = new IssueSetting()
            {
                Type = issueSettingViewModel.Type,
                Value = issueSettingViewModel.Value,
                Name = issueSettingViewModel.Name,
                Description = issueSettingViewModel.Description,
                ClassId = issueSettingViewModel.ClassId,
                Color = issueSettingViewModel.Color,
                Status = issueSettingViewModel.Status,
            };

            string checkDup = _isDAO.CheckDuplicate(iS);
            if (checkDup.Equals("Can Add"))
            {
                _isDAO.AddIssueSetting(iS);
                message.Success = "Add Issue Setting success";
            }
            message.Error = checkDup;
            return RedirectToAction("IssueSetting", new { id = issueSettingViewModel.ClassId });
        }
        [HttpPost("ToggleIssueSettingStatus")]
        public IActionResult ToggleIssueSettingStatus(int id,int classid)
        {
            var issueSetting = _isDAO.GetIssueSettingById(id);

            if (issueSetting == null)
            {
                return NotFound();
            }

            issueSetting.Status = !issueSetting.Status;

            _isDAO.UpdateIssueSetting(issueSetting);

            return RedirectToAction("IssueSetting",  new { id = classid} );
        }

       
        [HttpPost("ClosedMilestone")]
        public IActionResult CloseMilestone(int id,int classid)
        {
            var milestone = _milestoneService.GetMilestone(id);


            milestone.Status = !milestone.Status;
            var Class = _classService.GetClass(classid);
            _milestoneService.UpdateMilestone(milestone);
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
            IEnumerable<Milestone> Milestone = _classService.GetMilestone(id);
            ViewBag.MilestoneList = Milestone;
            return View("Milestones", ClassViewModel);
        }
    }
   
}
