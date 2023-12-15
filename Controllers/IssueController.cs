using Microsoft.AspNetCore.Mvc;

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
    public int? ParentIssueId { get; set; }
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
            try
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
                    case "MilestoneId":
                        issue.MilestoneId = selectedValue == 0 ? null : selectedValue;
                        break;
                    case "AssigneeId":
                        issue.AssigneeId = selectedValue == 0 ? null : selectedValue;
                        break;
                    case "ParentIssueId":
                        issue.ParentIssueId = selectedValue == 0 ? null : selectedValue;
                        break;
                    default:
                        return RedirectToAction("NotFound", "Error");
                }
                await context.SaveChangesAsync();
                return Json(new { success = true, type, message = type[..^2] + " updated successfully" });
            }
            catch
            {
                return Json(new { success = false, type, message = type[..^2] + " updated fail" });
            }
        }

        [Route("{projectId}/issues/{issueId:int}")]
        public IActionResult Detail(int projectId, int issueId)
        {
            var issue = context.Issues
                .Include(issue => issue.Type)
                .Include(issue => issue.Status)
                .Include(issue => issue.Process)
                .Include(issue => issue.Milestone)
                .Include(issue => issue.Author)
                .Include(issue => issue.Assignee)
                .Include(issue => issue.InverseParentIssue)
                .Include(issue => issue.ParentIssue)
                .FirstOrDefault(i => i.ProjectId == projectId && i.Id == issueId);

            var project = context.Projects
                .Include(p => p.Students)
                .Include(p => p.Issues)
                .Include(p => p.Milestones)
                .Include(p => p.Class).ThenInclude(cls => cls.Milestones)
                .SingleOrDefault(project => project.Id == projectId);

            if (project == null) return RedirectToAction("NotFound", "Error");

            var assignees = project.Students.ToList();
            var milestones = project.Milestones.Union(project.Class.Milestones).DistinctBy(milestone => milestone.Id).ToList();
            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();
            var issues = project.Issues.Where(issue => issue.Id != issueId).ToList();

            ViewBag.Types = types;
            ViewBag.Assignees = assignees;
            ViewBag.Milestones = milestones;
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;
            ViewBag.ParentIssues = issues;


            if (issue == null) return RedirectToAction("Index");
            return View(issue);
        }

        [Route("{projectId}/issues/{issueId:int}/edit")]
        public IActionResult Edit(int projectId, int issueId)
        {
            var issue = context.Issues
                .Include(issue => issue.Type)
                .Include(issue => issue.Status)
                .Include(issue => issue.Process)
                .Include(issue => issue.Milestone)
                .Include(issue => issue.Author)
                .Include(issue => issue.Assignee)
                .Include(issue => issue.InverseParentIssue)
                .Include(issue => issue.ParentIssue)
                .FirstOrDefault(i => i.ProjectId == projectId && i.Id == issueId);

            if (issue == null) return RedirectToAction("Index");

            var project = context.Projects
                .Include(p => p.Students)
                .Include(p => p.Issues)
                .Include(p => p.Milestones)
                .Include(p => p.Class).ThenInclude(cls => cls.Milestones)
                .SingleOrDefault(project => project.Id == projectId);

            if (project == null) return RedirectToAction("NotFound", "Error");

            var assignees = project.Students.ToList();
            var milestones = project.Milestones.Union(project.Class.Milestones).DistinctBy(milestone => milestone.Id).ToList();
            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();
            var issues = project.Issues.Where(issue => issue.Id != issueId).ToList();

            ViewBag.Types = types;
            ViewBag.Assignees = assignees;
            ViewBag.Milestones = milestones;
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;
            ViewBag.ParentIssues = issues;

            return View(issue);
        }

        [Route("{projectId}/issues/{issueId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(Issue issue, int projectId, int issueId, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                var issueToUpdate = await context.Issues.FirstOrDefaultAsync(x => x.Id == issueId);
                if (issueToUpdate == null) return RedirectToAction("NotFound", "Error");

                issueToUpdate.Title = issue.Title;
                issueToUpdate.TypeId = issue.TypeId;
                issueToUpdate.Description = issue.Description;
                issueToUpdate.AssigneeId = issue.AssigneeId;
                issueToUpdate.MilestoneId = issue.MilestoneId;
                issueToUpdate.StatusId = issue.StatusId;
                issueToUpdate.ProcessId = issue.ProcessId;
                issueToUpdate.ParentIssueId = issue.ParentIssueId;
                issueToUpdate.UpdatedAt = DateTime.Now;

                await context.SaveChangesAsync();
                errorHelper.Success = "Update issue successfull";
            }
            catch
            {
                errorHelper.Error = "Update issue faild";
            }
            return RedirectToAction("Detail", new { projectId, issueId });
        }

        [Route("{projectId}/issues/new")]
        [CustomAuthorize]
        public IActionResult Create(int projectId)
        {
            var project = context.Projects
                .Include(p => p.Students)
                .Include(p => p.Issues)
                .Include(p => p.Milestones)
                .Include(p => p.Class).ThenInclude(cls => cls.Milestones)
                .SingleOrDefault(project => project.Id == projectId);

            if (project == null) return RedirectToAction("NotFound", "Error");

            var assignees = project.Students.ToList();
            var milestones = project.Milestones.Union(project.Class.Milestones).DistinctBy(milestone => milestone.Id).ToList();
            var types = context.IssueSettings.Where(s => s.Type == "TYPE").ToList();
            var statuses = context.IssueSettings.Where(s => s.Type == "STATUS").ToList();
            var processes = context.IssueSettings.Where(s => s.Type == "PROCESS").ToList();
            var issues = project.Issues.ToList();

            ViewBag.Types = types;
            ViewBag.Assignees = assignees;
            ViewBag.Milestones = milestones;
            ViewBag.Statuses = statuses;
            ViewBag.Processes = processes;
            ViewBag.ParentIssues = issues;

            NewIssue vm = new()
            {
                ProjectName = project.Name
            };

            return View(vm);
        }

        [Route("{projectId}/issues/new")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<IActionResult> Create(NewIssue vm, int projectId, [FromServices] ErrorHelper errorHelper)
        {
            try
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
                    ParentIssueId = vm.ParentIssueId
                };

                context.Issues.Add(issue);
                await context.SaveChangesAsync();
                errorHelper.Success = "Create issue successfully";
                return RedirectToAction("Detail", new { projectId, issueId = issue.Id });
            }
            catch
            {
                errorHelper.Error = "Create issue faild";
                return RedirectToAction("Index");
            }
        }


        [Route("issues")]
        [CustomAuthorize]
        public IActionResult Index(IssueViewModel vm)
        {
            var userId = HttpContext.Session.GetUser()!.Id;

            User? user = context.Users.Include(user => user.Role).SingleOrDefault(user => user.Id == userId);
            if (user == null) return RedirectToAction("SignIn", "Auth");

            ViewBag.Projects = new List<Project>();
            ViewBag.Milestones = new List<Milestone>();
            ViewBag.Authors = new List<User>();
            ViewBag.Assignees = new List<User>();

            var userData = context.Users
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Class).ThenInclude(cls => cls.Milestones)
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Students)
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Milestones)
                 .AsSplitQuery()
                 .SingleOrDefault(user => user.Id == userId)!;

            var projects = userData.ProjectsNavigation.ToList();
            var milestones = userData.ProjectsNavigation.SelectMany(project => project.Class.Milestones).Union(userData.ProjectsNavigation.SelectMany(project => project.Milestones)).DistinctBy(milestone => milestone.Id).ToList();
            milestones.Insert(0, new Milestone() { Id = -1, Title = "Not yet" });
            var authors = userData.ProjectsNavigation.SelectMany(project => project.Students).DistinctBy(students => students.Id).ToList();
            var assignees = new List<User>(authors);
            assignees.Insert(0, new User() { Id = -1, Name = "Not yet" });

            ViewBag.Projects = projects;
            ViewBag.Milestones = milestones;
            ViewBag.Authors = authors;
            ViewBag.Assignees = assignees;

            if (projects.Count == 0) return View(vm);

            //-----------MAIN----------
            IQueryable<Issue> issues = context.Issues.AsQueryable().Where(issue => projects.Contains(issue.Project));


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
                    issues = issues.Where(issue => issue.Type.Value == "Requirement");
                    break;
                case "task":
                    issues = issues.Where(issue => issue.Type.Value == "Task");
                    break;
                case "question":
                    issues = issues.Where(issue => issue.Type.Value == "Q&A");
                    break;
                case "defect":
                    issues = issues.Where(issue => issue.Type.Value == "Defect");
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
    }
}