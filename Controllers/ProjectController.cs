using DocumentFormat.OpenXml.Office2010.Excel;
using IMS.DAO;
using IMS.Services;
using IMS.ViewModels.Validation;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IClassService _classService;

        public ProjectController(IProjectService projectService, IClassService classService)
        {
            _projectService = projectService;
            _classService = classService;
        }

        [Route("List")]
        [CustomAuthorize]
        public IActionResult Index(int? pageNumber, bool? filterByStatus, string? searchByName)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Project> paginate = new Paginate<Project>(tempPageNumber, tempPageSize);
            User user = HttpContext.Session.GetUser();

            List<Project> projects = new List<Project>();
            if (user.Role.Value == RoleUser.Admin)
            {
                projects = _projectService.GetAllProject();
            }
            if (user.Role.Value == RoleUser.Student) 
            {
                projects = _projectService.GetProjectByStudent(user.Id);
            }
            if (user.Role.Value == RoleUser.Teacher)
            {
                projects = _projectService.GetProjectByTeacher(user.Id);
            }

            // Filter và hoặc search -> quay về trang đầu
            if (filterByStatus != null)
            {
                projects = projects.Where(p => p.Status == filterByStatus).ToList();
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                projects = projects.Where(p => p.Name.Contains(searchByName)).ToList();
            }

            ViewBag.SearchValue = searchByName;
            ViewBag.ProjectList = paginate.GetListPaginate(projects);
            ViewBag.Action = "Index";
            ViewBag.User = user.Email;
            ViewBag.Pagination = paginate.GetPagination();
            return View();
        }


        [HttpPost("UpdateStatus")]
        public IActionResult ToggleStatus(int id)
        {
            var project = _projectService.GetProject(id);

            if (project == null)
            {
                return NotFound();
            }

            project.Status = !project.Status;

            _projectService.UpdateProject(project);

            return RedirectToAction("Index");
        }

        [Route("Create")]
        [CustomAuthorize]
        public IActionResult CreateProject()
        {
            User user = HttpContext.Session.GetUser();
            ViewBag.ClassList = _classService.GetClassesByStudent(5);
            return View();
        }

        [Route("Create"), HttpPost]
        [CustomAuthorize]
        public IActionResult CreateProject(ProjectViewModel? projectView, [FromServices] ErrorHelper errorMessage)
        {
            if (projectView == null)
            {
                return View();
            }

            User user = HttpContext.Session.GetUser();
            ViewBag.ClassList = _classService.GetClassesByStudent(5);

            if (!ModelState.IsValid)
            {
                return View();
            }

            Project project = new Project()
            {
                Name = projectView.Name,
                ClassId = projectView.ClassId,
                Description = projectView.Description,
                Status = projectView.Status,
            };

            project.LeaderId = user.Id;

            Class c = _classService.GetClass((int)project.ClassId);

            if (_projectService.CheckProjectExist(project))
            {
                ViewBag.ErrorMessage = "You have been created a project with name " + project.Name + " in class " + c.Name;
                return View();
            }

            _projectService.AddProject(project);
            errorMessage.Success = "Add Project success!";
            return RedirectToAction("Index");
        }

        [Route("Detail")]
        [CustomAuthorize]
        public IActionResult ProjectDetail(int id)
        {
            Project project = _projectService.GetProject(id);
            ProjectViewModel projectVM = new ProjectViewModel()
            {
                Id = id,
                ClassId = project.ClassId,
                Description = project.Description,
                Name = project.Name,
                Status = project.Status,
            };
            User user = HttpContext.Session.GetUser();
            if (user.Id == project.LeaderId)
            {
                ViewBag.Leader = true;
            }
            else
            {
                ViewBag.Leader = false;
            }
            ViewBag.Class = _classService.GetClass((int)project.ClassId);

            return View(projectVM);
        }

        [Route("Update"), HttpPost]
        public IActionResult ProjectUpdate(ProjectViewModel? projectView, [FromServices] ErrorHelper errorMessage)
        {
            if (projectView == null)
            {
                return RedirectToAction("ProjectDetail", new { id = projectView.Id });
            }

            User user = HttpContext.Session.GetUser();
            ViewBag.ClassList = _classService.GetClassesByStudent(user.Id);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ProjectDetail", new { id = projectView.Id });
            }

            Project project = _projectService.GetProject((int)projectView.Id);
            project.Description = projectView.Description;
            project.Name = projectView.Name;
            project.Status = projectView.Status;
            Class c = _classService.GetClass((int)project.ClassId);

            if (!_projectService.CheckProjectUpdate(project))
            {
                errorMessage.Error = "You have been created a project with name " + project.Name + " in class " + c.Name;
                return RedirectToAction("ProjectDetail", new { id = project.Id });
            }

            _projectService.UpdateProject(project);
            errorMessage.Success = "Update project success!";
            return RedirectToAction("ProjectDetail", new { id = project.Id } );
        }

        [HttpPost("CreateIssueSetting")]
        public IActionResult CreateIssueSetting(int id)
        {
            

            return RedirectToAction("IssueSetting");
        }

        [Route("IssueSetting")]
        public IActionResult IssueSetting(string? searchString)
        {
            

            return View();
        }
    }
}
