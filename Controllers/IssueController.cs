using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class IssueViewModel
{
    public string Tab { get; set; } = "all";
    public string Search { get; set; } = "";

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

        [Route("{projecId}/issues/new")]
        public IActionResult Create(int projecId)
        {

            var project = context.Projects
                .Include(p => p.Students)
                .Include(p => p.Class)
                .ThenInclude(cls => cls.Milestones)
                .SingleOrDefault(project => project.Id == projecId);

            if (project == null) return RedirectToAction("NotFound", "Error");

            NewIssue vm = new NewIssue()
            {
                ProjectName = project.Name
            };

            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();

            ViewBag.Types = types;
            ViewBag.Assignees = project.Students.ToList();
            ViewBag.Milestones = project.Class.Milestones.ToList();
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;

            return View(vm);
        }


        [Route("{projecId}/issues/new")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> Create(NewIssue vm, int projecId, [FromServices] ErrorHelper errorHelper)
        {
            var project = context.Projects.SingleOrDefault(project => project.Id == projecId);
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


            ViewBag.Projects = new List<Project>();
            ViewBag.Milestones = new List<Milestone>();
            ViewBag.Authors = new List<User>();
            ViewBag.Assignees = new List<User>();

            List<Class> classes = context.Classes
                .Include(cls => cls.Milestones)
                .Include(cls => cls.Projects)
                .ThenInclude(project => project.Students)
                .Where(cls => cls.TeacherId != null && cls.TeacherId == userId)
                .ToList();



            List<Project> projects = classes
                .SelectMany(classes => classes.Projects)
                .OrderBy(project => project.Name)
                .ToList();

            if (projects.Count == 0) return View(vm);

            List<Milestone> milestones = projects.SelectMany(project => project.Milestones).OrderBy(project => project.Title).ToList();
            List<User> users = projects.SelectMany(project => project.Students).DistinctBy(user => user.Id).OrderBy(user => user.Name).ToList();

            ViewBag.Projects = projects;
            milestones.Insert(0, new Milestone() { Id = -1, Title = "Not yet" });
            ViewBag.Milestones = milestones;
            ViewBag.Authors = users;

            var assignees = users.Select(user => user).ToList();
            assignees.Insert(0, new User() { Id = -1, Name = "Not yet" });
            ViewBag.Assignees = assignees;

            //
            IQueryable<Issue> issues = context.Issues.AsQueryable();

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

            var data = issues.ToList();

            if (!string.IsNullOrEmpty(vm.Search))
            {
                issues = issues.Where(issue => issue.Title.ToLower().Contains(vm.Search.ToLower()));
            }

            // Project
            if (vm.ProjectId == null)
            {
                issues = issues.Where(issue => projects.Contains(issue.Project));
            }
            else
            {
                issues = issues.Where(issue => issue.Project != null && issue.Project.Id == vm.ProjectId);
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
                issues = issues.Where(issue => issue.Author != null && issue.Author.Id == vm.AuthorId);
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


            //List<Issue> issues = new();
            //foreach (var project1 in projects)
            //{
            //    issues.AddRange(project1.Issues);
            //}
            //var userId = 4;

            //List<Project> projects = context.Projects
            //    .Include(project => project.Issues)
            //    .Include(project => project.IssueSettings)
            //    .Include(project => project.Milestones)
            //    .Include(project => project.Students)
            //    .Include(project => project.Leader)
            //    .Where(project => project.Students.Any(student => student.Id == userId))
            //    .ToList();

            //List<Issue> issues = new();
            //foreach (var project1 in projects)
            //{
            //    issues.AddRange(project1.Issues);
            //}

            //ViewBag.Projects = context.Projects.ToList();

            //IQueryable<Milestone> milestoneQuery = context.Milestones.AsQueryable();
            //List<Milestone> milestones = new();
            //if (vm.ProjectId != 0)
            //{
            //    milestones = milestoneQuery.Where(m => m.ProjectId == vm.ProjectId).ToList();
            //}
            //else
            //{
            //    var milestonesTemp = milestoneQuery.ToList();
            //    milestonesTemp.Insert(0, new Milestone() { Id = -1, Title = "Not yet" });
            //    milestones = milestonesTemp;
            //}
            //ViewBag.Milestones = milestones;

            //var project = context.Projects.Include(p => p.Students).SingleOrDefault(p => p.Id == vm.ProjectId);
            //List<User> authors = new();
            //if (project == null)
            //{
            //    authors = context.Users.ToList();
            //}
            //else
            //{
            //    authors = project.Students.ToList();
            //}

            //var users = context.Users.Where(teacher => teacher.Id == 4).SelectMany(teacher => teacher.Projects.SelectMany(project => project.Students)).DistinctBy(user => user.Id).ToList();
            //ViewBag.Authors = users;
            //ViewBag.Assignees = users;


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