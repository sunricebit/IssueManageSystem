using System;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Utilities;

public class SubjectSearchViewModel
{
    public string? Search { get; set; }
    public string? Type { get; set; }

    public SubjectSearchViewModel(string search, string type)
    {
        Search = search;
        Type = type;
    }

    [Required(ErrorMessage = "Please enter subject code")]
    [StringLength(10, ErrorMessage = "The code must be at least {2} characters long.", MinimumLength = 3)]
    [RegularExpression(@"^\S*$", ErrorMessage = "The code cannot contain whitespace.")]
    public string? Code { get; set; }

    [Required(ErrorMessage = "Please enter subject name")]
    public string? Name { get; set; }

    [StringLength(10, ErrorMessage = "The code must be at least {2} characters long.", MinimumLength = 3)]
    public string? Description { get; set; }


    [Display(Name = "Activate")]
    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Please select a manager")]
    [Display(Name = "Manager")]
    public int SubjectManagerId { get; set; } = 2;
}

public class AssignmentViewModel: AssignmentViewModel2
{
    public string? Search { get; set; }
    public string? Type { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int ItemCount { get; set; }

    public List<Assignment> Assignments { get; set; }

   
}

public class AssignmentViewModel2
{
    // 
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter assignment name")]
    public string Name { get; set; }

    [StringLength(10, ErrorMessage = "The code must be at least {2} characters long.", MinimumLength = 3)]
    public string? Description { get; set; }


    [Display(Name = "Activate")]
    public bool IsActive { get; set; }
}

namespace IMS.Controllers
{
    public class SubjectController : Controller
    {
        private readonly IMSContext _context;

        public SubjectController(IMSContext context)
        {
            _context = context;
        }

        [Route("/subjects")]
        public IActionResult Index(int? page, string? search, string? type, [FromServices] Intermediate intermediate)
        {
            var subjects = _context.Subjects.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                subjects = subjects.Where(subjects => subjects.Code.ToLower().Contains(search.ToLower()) || subjects.Name.ToLower().Contains(search.ToLower()));
            }

            switch (type)
            {
                case "activate":
                    subjects = subjects.Where(subject => subject.IsActive == true);
                    break;
                case "deactivate":
                    subjects = subjects.Where(subject => subject.IsActive == false);
                    break;
            }

            subjects = subjects.Include(subject => subject.SubjectManager).OrderByDescending(s => s.CreatedAt);
            ViewBag.SubjectManagers = new SelectList(_context.Users.Where(user => user.Role.Value == RoleUser.SubjectManager).ToList(), "Id", "Name");

            PaginateEnginee<Subject, SubjectSearchViewModel> a = PaginateEnginee<Subject, SubjectSearchViewModel>.Create(subjects, page ?? 1);
            a.Additional = new SubjectSearchViewModel(search ?? "", type ?? "");

            return View(a);
        }

        [Route("/subjects")]
        [HttpPost]
        public async Task<IActionResult> AddSubject(IFormCollection collection, int? page, string? search, string? type, [FromServices] Intermediate intermediate)
        {
            intermediate.Clear();

            string code = collection["Additional.Code"];
            string name = collection["Additional.Name"];
            string description = collection["Additional.Description"];

            var a = collection["isActive"].ToString();
            bool isActive = !string.IsNullOrEmpty(collection["isActive"].ToString());
            int subjectManagerId = int.Parse(collection["Additional.SubjectManagerId"]);

            var subject = _context.Subjects.SingleOrDefault(subject => subject.Code.ToLower().Equals(code.Trim().ToLower()));

            if (subject != null)
            {
                intermediate.Error = "Subject exist";
                intermediate.Code = code.ToUpper().Trim();
                intermediate.Name = name;
                intermediate.Description = description;
                intermediate.SubjectManagerId = subjectManagerId;
                return RedirectToAction("Index", new { page = page, search = search, type = type });
            }

            Subject newSujnect = new()
            {
                Code = code.ToUpper().Trim(),
                Name = name.Trim(),
                Description = description.Trim(),
                IsActive = isActive,
                SubjectManagerId = subjectManagerId
            };

            _context.Subjects.Add(newSujnect);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { page = page, search = search, type = type });
        }

        public async Task<IActionResult> Active(int subjectId, int? page, string? search, string? type)
        {
            var subject = _context.Subjects.SingleOrDefault(s => s.Id == subjectId);
            if (subject == null) return RedirectToAction("Index", new { page = page, search = search, type = type });
            subject.IsActive = !subject.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { page = page, search = search, type = type });
        }


        [Route("/subjects/{code}/information")]
        public IActionResult SubjectInformation(string code)
        {
            var subject = _context.Subjects.Include(s => s.SubjectManager).SingleOrDefault(s => s.Code == code);
            ViewBag.SubjectManagers = new SelectList(_context.Users.Where(user => user.Role.Value == RoleUser.SubjectManager).ToList(), "Id", "Name");
            if (subject == null) return RedirectToAction("NotFound", "Error");
            AddSubjectViewModel vm = new()
            {
                Code = subject.Code.Trim(),
                Name = subject.Name.Trim(),
                Description = subject.Description?.Trim(),
                SubjectManagerId = subject.SubjectManagerId,
                IsActive = subject.IsActive ?? false,
            };
            return View(vm);
        }

        [Route("/subjects/{code}/information")]
        [HttpPost]
        public async Task<IActionResult> SubjectInformation(AddSubjectViewModel vm, string code)
        {
            ViewBag.SubjectManagers = new SelectList(_context.Users.Where(user => user.Role.Value == RoleUser.SubjectManager).ToList(), "Id", "Name");
            if (!ModelState.IsValid) return View(vm);

            var subject = _context.Subjects.Include(s => s.SubjectManager).SingleOrDefault(s => s.Code == code);
            if (subject == null)
            {
                ViewBag.Error = "Subject not found!!!";
                return View(vm);
            };

            subject.Code = vm.Code.Trim();
            subject.Name = vm.Name.Trim();
            subject.Description = vm.Description?.Trim();
            subject.SubjectManagerId = vm.SubjectManagerId;
            subject.IsActive = vm.IsActive;

            ViewBag.Success = "Update subject successfully ";

            await _context.SaveChangesAsync();

            return View(vm);
        }

        [Route("/subjects/{code}/assignments")]
        [HttpGet]
        public IActionResult Assignments(string code, string? search, int? page, string? type)
        {
            var assignments = _context.Assignments.AsQueryable();
            assignments = assignments.Where(ass => ass.Subject != null && ass.Subject.Code.ToLower().Contains(code.ToLower()));

            if (!string.IsNullOrEmpty(search?.Trim()))
            {
                assignments = assignments.Where(ass => ass.Name.ToLower().Contains(search.Trim().ToLower()));
            }

            switch (type)
            {
                case "activate":
                    assignments = assignments.Where(assignment => assignment.IsActive == true);
                    break;
                case "deactivate":
                    assignments = assignments.Where(assignment => assignment.IsActive == false);
                    break;
            }

            int pageIndex = page ?? 1;
            int pageSize = 2;
            int itemCount = assignments.Count();
            int totalPages = (int)Math.Ceiling((double)itemCount / pageSize);

            if (pageIndex > totalPages) pageIndex = 1;
            assignments = assignments.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return View(new AssignmentViewModel() { Assignments = assignments.ToList(), Search = search, PageIndex = pageIndex, PageSize = pageSize, TotalPages = totalPages, ItemCount = itemCount });
        }

        public async Task<IActionResult> AssignmentsActive(string code, int assignmentId, int? page, string? search, string? type)
        {
            var assignment = _context.Assignments.SingleOrDefault(s => s.Id == assignmentId);
            if (assignment == null) return RedirectToAction("Assignments", new { code = code });
            assignment.IsActive = !assignment.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction("Assignments", new { code = code, page = page, search = search, type= type });
        }

        [Route("/subjects/{code}/assignments")]
        [HttpPost]
        public async Task<IActionResult> Assignments(AssignmentViewModel vm, string code)
        {
            var subject = _context.Subjects.SingleOrDefault(s => s.Code == code);
            if (subject == null) return NotFound();

            Assignment assignment = new Assignment()
            {
                Name = vm.Name,
                Description = vm.Description,
                IsActive = vm.IsActive,
                SubjectId = subject.Id
            };
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Assignments", new { code = code });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssignment(AssignmentViewModel2 vm, string code, int? page, string? search, string? type)
        {
            var assignment = _context.Assignments.SingleOrDefault(ass => ass.Id == vm.Id);
            if(assignment == null) return RedirectToAction("Assignments", new { code = code, page = page, search = search, type = type });

            assignment.Name = vm.Name;
            assignment.Description = vm.Description;
            assignment.IsActive = vm.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction("Assignments", new { code = code, page = page, search = search, type = type });
        }
    }
}