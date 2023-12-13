using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IssueViewModel
{
    public string Tab { get; set; } = "all";
    public string? Search { get; set; } = "";

    [Display(Name = "Project")]
    public int? ProjectId { get; set; }
    [Display(Name = "Milestone")]
    public int? MilestoneId { get; set; }
    [Display(Name = "Author")]
    public int? AuthorId { get; set; }
    [Display(Name = "Assignee")]
    public int? AssigneeId { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int ItemCount { get; set; }
    public int TotalPages { get; set; }
    public List<Issue> Issues { get; set; } = new();
}

public class NewIssue
{
    [Display(Name = "Project name")]
    public string ProjectName { get; set; }

    [Required(ErrorMessage = "Please enter issue title")]
    public string Title { get; set; }

    [Required]
    [Display(Name = "Type")]
    public int TypeId { get; set; }

    public string? Description { get; set; }

    [Display(Name = "Assignee")]
    public int? AssigneeId { get; set; }

    [Display(Name = "Milestone")]
    public int? MilestoneId { get; set; }

    [Required(ErrorMessage = "Please select an issue status")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Please select an issue process")]
    [Display(Name = "Process")]
    public int ProcessId { get; set; }

    [Display(Name = "Issue parent")]
    public int? IssueParentId { get; set; }
}

namespace IMS.Controllers
{
    public class IssueController : Controller
    {
        private readonly IMSContext context;

        public IssueController(IMSContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int issueId, string type, int selectedValue)
        {
            var issue = context.Issues.SingleOrDefault(issue => issue.Id == issueId);
            if (issue == null) return RedirectToAction("NotFound", "Error");

            switch (type)
            {
                case "StatusId":
                    issue.StatusId = selectedValue;
                    break;
                case "TypeId":
                    issue.TypeId = selectedValue;
                    break;
                case "ProcessId":
                    issue.ProcessId = selectedValue;
                    break;
                case "AssigneeId":
                    issue.AssigneeId = selectedValue;
                    break;
                case "ParentIssueId":
                    issue.ParentIssueId = selectedValue;
                    break;
                default:
                    return RedirectToAction("NotFound", "Error");
            }
            await context.SaveChangesAsync();
            return Json(new { success = true, type, message = type[..^2] + " updated successfully" });
        }

        [Route("{projectId}/issues/{issueId:int}")]
        public IActionResult Detail(int projectId, int issueId)
        {
            var issue = context.Issues
                .Include(issue => issue.Type)
                .Include(issue => issue.Status)
                .Include(issue => issue.Process)
                .Include(issue => issue.Process)
                .Include(issue => issue.Milestone)
                .Include(issue => issue.Author)
                .Include(issue => issue.Assignee)
                .Include(issue => issue.InverseParentIssue)
                .Include(issue => issue.ParentIssue)
                .FirstOrDefault(i => i.ProjectId == projectId && i.Id == issueId);

            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();
            var issues = context.Issues.Where(issue => issue.ProjectId == projectId && issue.Id != issueId);
            var assignees = context.Projects.Include(project => project.Students).FirstOrDefault(project => project.Id == projectId)!.Students.ToList();


            ViewBag.Types = types;
            ViewBag.Assignees = assignees;
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;
            ViewBag.Issues = issues;


            if (issue == null) return RedirectToAction("Index");
            return View(issue);
        }


        [Route("{projectId}/issues/new")]
        public IActionResult Create(int projectId)
        {
            var project = context.Projects
                .Include(p => p.Students)
                .Include(p => p.Class)
                .ThenInclude(cls => cls.Milestones)
                .SingleOrDefault(project => project.Id == projectId);

            if (project == null) return RedirectToAction("NotFound", "Error");

            NewIssue vm = new()
            {
                ProjectName = project.Name
            };

            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();
            var issues = context.Issues.Where(issue => issue.ProjectId == projectId);

            ViewBag.Types = types;
            ViewBag.Assignees = project.Students.ToList();
            ViewBag.Milestones = project.Class.Milestones.ToList();
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;
            ViewBag.Issues = issues;

            return View(vm);
        }


        [Route("{projectId}/issues/new")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> Create(NewIssue vm, int projectId, [FromServices] ErrorHelper errorHelper)
        {
            var project = context.Projects.SingleOrDefault(project => project.Id == projectId);
            if (project == null) return RedirectToAction("NotFound", "Error");

            int userId = HttpContext.Session.GetUser()!.Id;
            Issue issue = new()
            {
                Title = vm.Title,
                Description = vm.Description,
                TypeId = vm.TypeId,
                StatusId = vm.StatusId,
                ProcessId = vm.ProcessId,
                MilestoneId = vm.MilestoneId,
                ProjectId = project.Id,
                AuthorId = userId,
                AssigneeId = vm.AssigneeId,
                ParentIssueId = vm.IssueParentId
            };

            context.Issues.Add(issue);
            await context.SaveChangesAsync();
            errorHelper.Success = "Create issue successfully";

            return RedirectToAction("Index");
        }


        [Route("issues")]
        [CustomAuthorize]
        public IActionResult Index(IssueViewModel vm)
        {
            var userId = HttpContext.Session.GetUser()!.Id;

            //kiểm tra user có thực sự tồn tại
            User? user = context.Users.Include(user => user.Role).SingleOrDefault(user => user.Id == userId);
            if (user == null) return RedirectToAction("SignIn", "Auth");

            ViewBag.Projects = new List<Project>();
            ViewBag.Milestones = new List<Milestone>();
            ViewBag.Authors = new List<User>();
            ViewBag.Assignees = new List<User>();

            var roleName = user.Role.Value;

            //lấy tất cả các project đã tham gia/
            List<Project> projectJoined = new();
            switch (roleName)
            {
                case "Teacher":
                    projectJoined = context.Classes
                        .Include(cls => cls.Projects)
                        .ThenInclude(project => project.Students)
                        .Where(cls => cls.TeacherId != null && cls.TeacherId == userId)
                        .SelectMany(cls => cls.Projects)
                        .OrderBy(project => project.Name)
                        .ToList();
                    break;
                case "Student":
                    projectJoined = context.Users
                        .Include(t => t.ProjectsNavigation)
                        .ThenInclude(project => project.Students)
                        .SingleOrDefault(t => t.Id == userId)!
                        .ProjectsNavigation
                        .OrderBy(project => project.Name)
                        .ToList();
                    break;
                default:
                    return RedirectToAction("Index");
            };
            if (projectJoined.Count == 0) return View(vm);
            ViewBag.Projects = projectJoined;

            //lấy tất cả các milestone
            List<Milestone> milestones = new();
            switch (roleName)
            {
                case "Teacher":
                    var teacher = context.Users.Include(t => t.Classes).ThenInclude(cls => cls.Milestones).SingleOrDefault(t => t.Id == userId);
                    if (teacher != null) milestones = teacher.Classes.SelectMany(c => c.Milestones).ToList();
                    break;
                case "Student":
                    var student = context.Users.Include(t => t.ClassesNavigation).ThenInclude(cls => cls.Milestones).SingleOrDefault(t => t.Id == userId);
                    if (student != null) milestones = student.Classes.SelectMany(c => c.Milestones).ToList();
                    break;
                default:
                    return RedirectToAction("Index");
            };

            ViewBag.Milestones = milestones;

            //Get author
            var authors = projectJoined.SelectMany(project => project.Students).DistinctBy(user => user.Id).OrderBy(user => user.Name);
            ViewBag.Authors = authors;

            //Get assignees
            var assignees = authors.Select(user => user).ToList();
            assignees.Insert(0, new User() { Id = -1, Name = "Not yet" });
            ViewBag.Assignees = assignees;


            //-----------MAIN----------
            IQueryable<Issue> issues = context.Issues.AsQueryable().Where(issue => projectJoined.Contains(issue.Project));


            if (!string.IsNullOrEmpty(vm.Search?.Trim()))
            {
                issues = issues.Where(issue => issue.Title.ToLower().Contains(vm.Search.Trim().ToLower()));
            }

            // Project
            if (vm.ProjectId != null)
            {
                issues = issues.Where(issue => issue.Project.Id == vm.ProjectId);
            }

            //Milestone
            if (vm.MilestoneId != null)
            {
                if (vm.MilestoneId == -1)
                {
                    issues = issues.Where(issue => issue.MilestoneId == null);
                }
                else
                {
                    issues = issues.Where(issue => issue.MilestoneId != null && issue.MilestoneId == vm.MilestoneId);
                }
            }

            //Author
            if (vm.AuthorId != null)
            {
                issues = issues.Where(issue => issue.AuthorId == vm.AuthorId);
            }

            //Assignee
            if (vm.AssigneeId != null)
            {
                if (vm.AssigneeId == -1)
                {
                    issues = issues.Where(issue => issue.AssigneeId == null);
                }
                else
                {
                    issues = issues.Where(issue => issue.AssigneeId != null && issue.AssigneeId == vm.AssigneeId);
                }
            }

            switch (vm.Tab)
            {
                case "requirement":
                    issues = issues.Where(issue => issue.Type.Value == "R");
                    break;
                case "task":
                    issues = issues.Where(issue => issue.Type.Value == "T");
                    break;
                case "question":
                    issues = issues.Where(issue => issue.Type.Value == "Q");
                    break;
                case "defect":
                    issues = issues.Where(issue => issue.Type.Value == "D");
                    break;
            }

            issues = issues
                .Include(issue => issue.Type)
                .Include(issue => issue.Status)
                .Include(issue => issue.Process)
                .Include(issue => issue.Process)
                .Include(issue => issue.Milestone)
                .Include(issue => issue.Author)
                .Include(issue => issue.Assignee)
                .OrderByDescending(issue => issue.CreatedAt);

            int pageIndex = vm.PageIndex == 0 ? 1 : vm.PageIndex;
            int pageSize = 10;
            int itemCount = issues.Count();
            int totalPages = (int)Math.Ceiling((double)itemCount / pageSize);

            issues = issues.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            vm.PageIndex = pageIndex;
            vm.PageSize = pageSize;
            vm.ItemCount = itemCount;
            vm.TotalPages = totalPages;
            vm.Issues = issues.ToList();

            return View(vm);
        }




        [HttpGet]
        public IActionResult GetProjectByName(string? search)
        {
            if (search == null) return Json("no-item");
            var project = context.Projects.Where(project => project.Name.ToLower().Contains(search.ToLower())).Select(p => new { p.Id, p.Name }).ToList();
            return Json(project);
        }

        public IActionResult GetMilestones(int projectId)
        {
            var project = context.Projects.Include(project => project.Milestones).SingleOrDefault(project => project.Id == projectId);
            if (project == null) RedirectToAction("Index");
            return Json(project!.Milestones);
        }
    }
}