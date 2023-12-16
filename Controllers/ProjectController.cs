using DocumentFormat.OpenXml.Office2010.Excel;
using IMS.Models;
using IMS.ViewModels.Milestone;
using IMS.ViewModels.Validation;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IClassService _classService;
        private readonly IssueSettingDAO _isDAO;
        private readonly IMilestoneService _milestoneService;
      

        public ProjectController(IProjectService projectService, IClassService classService, IssueSettingDAO isDAO, IMilestoneService milestoneService)
        {
            _projectService = projectService;
            _classService = classService;
            _isDAO = isDAO;
            _milestoneService = milestoneService;
        }

        [Route("List")]
        [CustomAuthorize]
        public IActionResult Index(int? pageNumber, int? filterByClass, bool? filterByStatus, string? searchByName, [FromServices]IChkPgAcessService chkPgAcess)
        {
            int tempPageNumber = pageNumber ?? 1;
            int tempPageSize = 10;
            Paginate<Project> paginate = new Paginate<Project>(tempPageNumber, tempPageSize);
            User user = HttpContext.Session.GetUser();

            List<Project> projects = new List<Project>();
            if (user.Role.Value == RoleUser.Admin)
            {
                projects = _projectService.GetAllProject();
                ViewBag.ClassList = _classService.GetClasses();
            }
            if (user.Role.Value == RoleUser.Student) 
            {
                projects = _projectService.GetProjectByStudent(user.Id);
                ViewBag.ClassList = _classService.GetClassesByTeacher(user.Id);
            }
            if (user.Role.Value == RoleUser.Teacher)
            {
                projects = _projectService.GetProjectByTeacher(user.Id);
                ViewBag.ClassList = _classService.GetClassesByTeacher(user.Id);
            }

            if (filterByStatus != null)
            {
                projects = projects.Where(p => p.Status == filterByStatus).ToList();
            }

            if (filterByClass != null)
            {
                projects = projects.Where(p => p.ClassId == filterByClass).ToList();
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                projects = projects.Where(p => p.Name.Contains(searchByName)).ToList();
            }

            ViewBag.SearchValue = searchByName;
            ViewBag.ProjectList = paginate.GetListPaginate(projects);
            ViewBag.Action = "Index";
            ViewBag.Pagination = paginate.GetPagination();
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
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
            List<Class> classList = _classService.GetClassesByTeacher(user.Id).ToList();
            ViewBag.ClassList = classList;
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

            if (!ModelState.IsValid)
            {
                return View();
            }

            Project project = new Project()
            {
                Name = projectView.Name,
                ClassId = projectView.ClassId,
                GroupName = projectView.GroupName,
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
        public IActionResult ProjectDetail(int id, [FromServices] IChkPgAcessService chkPgAcess)
        {
            Project project = _projectService.GetProject(id);
            ProjectViewModel projectVM = new ProjectViewModel()
            {
                Id = id,
                ClassId = project.ClassId,
                Description = project.Description,
                GroupName = project.GroupName,
                
                Name = project.Name,
                Status = project.Status,
            };
            User user = HttpContext.Session.GetUser();
            if (user.Id == project.LeaderId || user.Role.Value.Equals(RoleUser.Teacher))
            {
                ViewBag.Leader = true;
            }
            else
            {
                ViewBag.Leader = false;
            }
            ViewBag.Class = _classService.GetClass((int)project.ClassId).Name;
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);
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
            //ViewBag.ClassList = _classService.GetClassesByStudent(user.Id);

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ProjectDetail", new { id = projectView.Id });
            }

            Project project = _projectService.GetProject((int)projectView.Id);
            project.Description = projectView.Description;
            project.Name = projectView.Name;
            project.GroupName = projectView.GroupName;
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
        public IActionResult CreateIssueSetting(IssueSettingViewModel issueSettingViewModel, [FromServices] ErrorHelper message)
        {
            Project p = _projectService.GetProject((int)issueSettingViewModel.ProjectId);
            IssueSetting iS = new IssueSetting()
            {
                Type = issueSettingViewModel.Type,
                Value = issueSettingViewModel.Value,
                Name = issueSettingViewModel.Name,
                Description = issueSettingViewModel.Description,
                ProjectId = issueSettingViewModel.ProjectId,
                ClassId = p.ClassId,
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

            return RedirectToAction("IssueSetting", new { id = p.Id });
        }

        [Route("Member")]
        [CustomAuthorize]
        public IActionResult Member(string? searchString, int id,string filterbyStatus, 
            [FromServices] IChkPgAcessService chkPgAcessService, [FromServices] ErrorHelper message)
        {
            ViewBag.ProjectId = id;
            //ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            //ViewBag.SuccessMessage = TempData["SuccessMessage"] as string; 
            message.Error = TempData["ErrorMessage"] as string;
            message.Success = TempData["SuccessMessage"] as string;

            User u = HttpContext.Session.GetUser();
            Project p = _projectService.GetProject(id);
            ViewBag.StudentInClass = _classService.GetStudentInClass(p.ClassId);
            var members = _projectService.GetStudentInProject(id);

            if (!string.IsNullOrEmpty(filterbyStatus) && filterbyStatus != "ALL")
            {
                if (filterbyStatus == "true")
                {
                    members = members.Where(item => item.Status == true).ToList();
                }
                else
                {
                    members = members.Where(item => item.Status == false).ToList();
                }
            }
            if (!string.IsNullOrEmpty(searchString))    
            {
               
                members = members.Where(item => item.Name.ToLower().Contains(searchString.ToLower())
                || item.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }
            ViewBag.PageAccess = chkPgAcessService.GetPageAccess(HttpContext);
            ViewBag.Student = members;
            return View();
        }

        [HttpPost("AddStudent")]
        public IActionResult AddStudent(int projectid, string Name, [FromServices] ErrorHelper errorMessage)
        {
            if (_projectService.AddStudentToProject(projectid, Name) == false)
            {
                errorMessage.Error = "Student is already exist.";
                return RedirectToAction("Member", new { id = projectid });
            }
            else
            {
                errorMessage.Success = "Student added to the project successfully.";
                return RedirectToAction("Member", new { id = projectid });
            }
        }

        [HttpPost("RemoveStudent")]
        public IActionResult RemoveStudent(int id, string email)
        {
            
            _projectService.RemoveStudentFromProject(id, email);

            return RedirectToAction("Member", new {id = id});
        }

        [Route("IssueSetting")]
        [CustomAuthorize]
        public IActionResult IssueSetting(string? searchString, int id)
        {
            ViewBag.ProjectId = id;
            User u = HttpContext.Session.GetUser();
            Project p = _projectService.GetProject(id);

            ViewBag.ProjectId = id;

            List<IssueSetting> issueSettings = _isDAO.GetIssueSettingByProject(id);
            if (!string.IsNullOrEmpty(searchString))
            {
                issueSettings = issueSettings.Where(item => item.Type.ToLower().Contains(searchString.ToLower())
                || item.Value.ToLower().Contains(searchString.ToLower())).ToList();
            }
            ViewBag.IssueSettingList = issueSettings;
            return View();
        }

        [HttpPost("ToggleIssueSettingStatus")]
        public IActionResult ToggleIssueSettingStatus(int id, int projectId)
        {
            var issueSetting = _isDAO.GetIssueSettingById(id);

            if (issueSetting == null)
            {
                return NotFound();
            }

            issueSetting.Status = !issueSetting.Status;

            _isDAO.UpdateIssueSetting(issueSetting);

            return RedirectToAction("IssueSetting", new { id = projectId });
        }

        [HttpGet("Milestones/{id}")]
        [CustomAuthorize]
        public IActionResult ProjectMilestone(int id, string searchString, [FromServices] IChkPgAcessService chkPgAcess)
        {
            var milestone = _classService.GetMilestoneByProject(id);
            var project = _projectService.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }
            ProjectViewModel projectViewModel = new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ClassId = project.ClassId,
                GroupName = project.GroupName,
                Status = project.Status,
            };
            ViewBag.PageAccess = chkPgAcess.GetPageAccess(HttpContext);

            if (!string.IsNullOrEmpty(searchString))
            {
                milestone = milestone.Where(item => item.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }
            ViewBag.MilestoneList = milestone;
            return View(projectViewModel);
        }


        [HttpPost("CreateMilestone")]
        public IActionResult CreateMilestone(MilestoneViewModel model, [FromServices] ErrorHelper errorMessage)
        {
            var project = _projectService.GetProject((int)model.ProjectId);
            Milestone milestone = new Milestone()
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ClassId = project.ClassId,
                ProjectId = model.ProjectId,
            };
            milestone.Status = false;
            _milestoneService.AddMilestone(milestone);
            errorMessage.Success = "Add milestone success. Please reload this page.";
            return RedirectToAction("ProjectMilestone", new { id = model.ProjectId  });
        }

        [HttpPost("ClosedMilestone")]
        public IActionResult CloseMilestone(int id)
        {
            var milestone = _milestoneService.GetMilestone(id);

            milestone.Status = !milestone.Status;
            var project = _projectService.GetProject(id);
            _milestoneService.UpdateMilestone(milestone);
            ProjectViewModel projectViewModel = new ProjectViewModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                ClassId = project.ClassId,
                GroupName = project.GroupName,
                Status = project.Status,
            };
            IEnumerable<Milestone> Milestone = _classService.GetMilestoneByProject(id);
            ViewBag.MilestoneList = Milestone;
            return View("ProjectMilestone", new { id = project.Id });
        }
    }
}
