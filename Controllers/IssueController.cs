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

namespace IMS.Controllers
{
    public class IssueController : Controller
    {
        private readonly IMSContext context;

        public IssueController(IMSContext _context)
        {
            context = _context;
        }

        [Route("issues")]
        public IActionResult Index(IssueViewModel vm)
        {

            var userId = 4;

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

            if (classes.Count == 0) return View(vm);
            
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
                    issues = issues.Where(issue => issue.Type.Name == "R");
                    break;
                case "task":
                    issues = issues.Where(issue => issue.Type.Name == "T");
                    break;
                case "question":
                    issues = issues.Where(issue => issue.Type.Name == "Q");
                    break;
                case "defect":
                    issues = issues.Where(issue => issue.Type.Name == "D");
                    break;
            }

            if (!string.IsNullOrEmpty(vm.Search))
            {
                issues = issues.Where(issue => issue.Title.ToLower().Contains(vm.Search.ToLower()));
            }

            // Project
            if (vm.ProjectId == 0)
            {
                issues = issues.Where(issue => projects.Contains(issue.Project));
            }
            else
            {
                issues = issues.Where(issue => issue.Project != null && issue.Project.Id == vm.ProjectId);
            }

            
            //Milestone
            if (vm.MilestoneId != 0)
            {
                if (vm.MilestoneId == -1)
                {
                    issues = issues.Where(issue => issue.MilestoneId == null);
                }
                else
                {
                    issues = issues.Where(issue => issue.Milestone != null && issue.Milestone.Id == vm.MilestoneId);
                }
            }

            //Author
            if (vm.AuthorId != 0)
            {
                issues = issues.Where(issue => issue.Author != null && issue.Author.Id == vm.AuthorId);
            }

            //Assignee
            if (vm.AssigneeId != 0)
            {
                if (vm.MilestoneId == -1)
                {
                    issues = issues.Where(issue => issue.AssigneeId == null);
                }
                else
                {
                    issues = issues.Where(issue => issue.Assignee != null && issue.Assignee.Id == vm.AssigneeId);
                }
            }

            issues = issues.Include(issue => issue.Type).Include(issue => issue.Status).Include(issue => issue.Process);

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

        [HttpPost]
        public IActionResult Check(IssueViewModel vm)
        {
            return RedirectToAction("Index");
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