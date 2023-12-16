using System.Linq;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class IssueViewModel
{
    public string Tab { get; set; } = "A";
    public string? Search { get; set; } = "";

    [Display(Name = "Project")]
    public int? ProjectId { get; set; }
    [Display(Name = "Status")]
    public int? StatusId { get; set; }
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

    [Display(Name = "Document")]
    public IFormFile? File { get; set; }
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
        public async Task<IActionResult> Edit(Issue issue, IFormFile? file, int projectId, int issueId, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                var issueToUpdate = await context.Issues.FirstOrDefaultAsync(x => x.Id == issueId);
                if (issueToUpdate == null) return RedirectToAction("NotFound", "Error");



                if (file != null && file.Length > 0)
                {
                    if (issue.DocumentUrl != null && issue.FileName != null)
                    {
                        await DeleteDocument(issue.FileName);
                    }

                    string? documentUrl = null;

                    using var stream = file.OpenReadStream();
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var downloadLink = await UploadFromFirebase(stream, fileName);

                    documentUrl = downloadLink;
                }

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

                string? documentUrl = null;
                string? filename = null;

                if (vm.File != null && vm.File.Length > 0)
                {
                    using var stream = vm.File.OpenReadStream();
                    filename = Guid.NewGuid().ToString() + Path.GetExtension(vm.File.FileName);
                    var downloadLink = await UploadFromFirebase(stream, filename);

                    documentUrl = downloadLink;
                }


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
                    ParentIssueId = vm.ParentIssueId,
                    DocumentUrl = documentUrl,
                    FileName = filename
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

        public async Task<string> UploadFromFirebase(Stream stream, string fileName)
        {
            string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
            string Bucket = "imsmanagement-35781.appspot.com";
            string AuthEmail = "abc@gmail.com";
            string AuthPassword = "123456";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var token = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            if (token == null) return "";
            string accessToken = token.FirebaseToken;
            var firebaseStorage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)
            });

            var path = $"documents/{fileName}";

            var task = await firebaseStorage.Child(path).PutAsync(stream);

            var downloadUrl = await firebaseStorage.Child(path).GetDownloadUrlAsync();
            return downloadUrl;
        }

        public async Task<string> DeleteDocument(string fileName)
        {
            string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
            string Bucket = "imsmanagement-35781.appspot.com";
            string AuthEmail = "abc@gmail.com";
            string AuthPassword = "123456";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

            var token = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            if (token == null) return "";
            string accessToken = token.FirebaseToken;
            var firebaseStorage = new FirebaseStorage(Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token.FirebaseToken)
            });

            var path = $"documents/{fileName}";

            await firebaseStorage.Child(path).DeleteAsync();

            var downloadUrl = await firebaseStorage.Child(path).GetDownloadUrlAsync();
            await auth.SignInWithOAuthAsync(FirebaseAuthType.EmailAndPassword, accessToken);
            return downloadUrl;
        }


        public IActionResult GetDataByProjectId(int? projectId)
        {
            if (projectId.HasValue)
            {
                var project = context.Projects
                    .Where(p => p.Id == projectId)
                    .Select(p => new
                    {
                        Students = p.Students.Select(s => new { s.Id, s.Name }),
                        Issues = p.Issues.Select(i => new { i.Id, i.Title }),
                        Milestones = p.Milestones.Select(m => new { m.Id, m.Title }),
                        Class = new { Milestones = p.Class.Milestones.Select(cm => new { cm.Id, cm.Title }) }
                    })
                    .SingleOrDefault();

                if (project == null) return RedirectToAction("NotFound", "Error");

                var authors = project.Students.ToList();
                var assignees = authors.Select(user => user).ToList();
                assignees.Insert(0, new { Id = -1, Name = "Not yet" });

                var milestones = project.Milestones.Union(project.Class.Milestones).DistinctBy(milestone => milestone.Id).ToList();

                string authorsJson = JsonConvert.SerializeObject(authors, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string assigneesJson = JsonConvert.SerializeObject(assignees, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string milestonesJson = JsonConvert.SerializeObject(milestones, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(new { authorsJson, assigneesJson, milestonesJson });
            }
            else
            {
                var userId = HttpContext.Session.GetUser()!.Id;
                Models.User? user = context.Users.SingleOrDefault(user => user.Id == userId);
                if (user == null) return RedirectToAction("SignIn", "Auth");

                var userData = context.Users
                    .Where(user => user.Id == userId)
                    .Select(user => new
                    {
                        ProjectsNavigation = user.ProjectsNavigation.Select(project => new
                        {
                            project.Id,
                            project.Name,
                            Class = new { Milestones = project.Class.Milestones.Select(clsMilestone => new { clsMilestone.Id, clsMilestone.Title }) },
                            Students = project.Students.Select(student => new { student.Id, student.Name }),
                            Milestones = project.Milestones.Select(projectMilestone => new { projectMilestone.Id, projectMilestone.Title })
                        })
                    })
                    .AsSplitQuery()
                    .SingleOrDefault()!;

                var milestones = userData.ProjectsNavigation.SelectMany(project => project.Class.Milestones).Union(userData.ProjectsNavigation.SelectMany(project => project.Milestones)).DistinctBy(milestone => milestone.Id).ToList();
                milestones.Insert(0, new { Id = -1, Title = "Not yet" });
                var authors = userData.ProjectsNavigation.SelectMany(project => project.Students).DistinctBy(students => students.Id).ToList();
                var assignees = authors.Select(user => user).ToList();
                assignees.Insert(0, new { Id = -1, Name = "Not yet" });

                string authorsJson = JsonConvert.SerializeObject(authors, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string assigneesJson = JsonConvert.SerializeObject(assignees, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                string milestonesJson = JsonConvert.SerializeObject(milestones, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(new { authorsJson, assigneesJson, milestonesJson });
            }
        }



        [Route("issues")]
        [CustomAuthorize]
        public IActionResult Index(IssueViewModel vm)
        {
            var userId = HttpContext.Session.GetUser()!.Id;

            Models.User? user = context.Users.Include(user => user.Role).SingleOrDefault(user => user.Id == userId);
            if (user == null) return RedirectToAction("SignIn", "Auth");

            ViewBag.Projects = new List<Project>();
            ViewBag.Milestones = new List<Milestone>();
            ViewBag.Authors = new List<Models.User>();
            ViewBag.Assignees = new List<Models.User>();
            ViewBag.Statuses = context.IssueSettings.Where(setting => setting.Type == "STATUS").ToList();

            var userData = context.Users
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Class).ThenInclude(cls => cls.Milestones)
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Students)
                 .Include(user => user.ProjectsNavigation).ThenInclude(project => project.Milestones)
                 .AsSplitQuery()
                 .SingleOrDefault(user => user.Id == userId)!;

            var projects = userData.ProjectsNavigation.ToList();
            ViewBag.Projects = projects;
            if (projects.Count == 0) return View(vm);


            if (vm.ProjectId.HasValue)
            {
                var project = context.Projects
                     .Where(p => p.Id == vm.ProjectId)
                     .Select(p => new
                     {
                         Students = p.Students.Select(s => new { s.Id, s.Name }),
                         Issues = p.Issues.Select(i => new { i.Id, i.Title }),
                         Milestones = p.Milestones.Select(m => new { m.Id, m.Title }),
                         Class = new { Milestones = p.Class.Milestones.Select(cm => new { cm.Id, cm.Title }) }
                     })
                     .SingleOrDefault();

                if (project == null) return RedirectToAction("NotFound", "Error");

                var milestones = project.Milestones.Union(project.Class.Milestones).DistinctBy(milestone => milestone.Id).ToList();
                milestones.Insert(0, new { Id = -1, Title = "Not yet" });
                var authors = project.Students.ToList();
                var assignees = authors.Select(user => user).ToList();
                assignees.Insert(0, new { Id = -1, Name = "Not yet" });

                ViewBag.Milestones = milestones;
                ViewBag.Authors = authors;
                ViewBag.Assignees = assignees;

            }
            else
            {
                var userdData2 = context.Users
                    .Where(user => user.Id == userId)
                    .Select(user => new
                    {
                        ProjectsNavigation = user.ProjectsNavigation.Select(project => new
                        {
                            project.Id,
                            project.Name,
                            Class = new { Milestones = project.Class.Milestones.Select(clsMilestone => new { clsMilestone.Id, clsMilestone.Title }) },
                            Students = project.Students.Select(student => new { student.Id, student.Name }),
                            Milestones = project.Milestones.Select(projectMilestone => new { projectMilestone.Id, projectMilestone.Title })
                        })
                    })
                    .AsSplitQuery()
                    .SingleOrDefault()!;

                var milestones = userdData2.ProjectsNavigation.SelectMany(project => project.Class.Milestones).Union(userdData2.ProjectsNavigation.SelectMany(project => project.Milestones)).DistinctBy(milestone => milestone.Id).ToList();
                milestones.Insert(0, new { Id = -1, Title = "Not yet" });
                var authors = userdData2.ProjectsNavigation.SelectMany(project => project.Students).DistinctBy(students => students.Id).ToList();
                var assignees = authors.Select(user => user).ToList();
                assignees.Insert(0, new { Id = -1, Name = "Not yet" });

                ViewBag.Milestones = milestones;
                ViewBag.Authors = authors;
                ViewBag.Assignees = assignees;
            }


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

            if (vm.StatusId != null)
            {
                issues = issues.Where(issue => issue.StatusId == vm.StatusId);
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

            if (vm.Tab != "A")
            {
                issues = issues.Where(issue => issue.Type.Value == vm.Tab);
            }

            var types = context.IssueSettings.Where(setting => setting.Type == "TYPE").ToList();
            types.Insert(0, new IssueSetting() { Value = "A", Name = "All" });
            ViewBag.Types = types;

            issues = issues
                .Include(issue => issue.Type)
                .Include(issue => issue.Status)
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