using System;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.Common;
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

public class AssignmentViewModel : AssignmentViewModel2
{
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

    [Required(ErrorMessage = "Please enter weight assignment")]
    [Range(1, 100, ErrorMessage = "The value must be greater than 0 and less than or equal to 100")]
    public int Weight { get; set; }
}

public class SubjectListViewModel
{
    public string? Search { get; set; }
    public string Type { get; set; } = "all";

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int ItemCount { get; set; }
    public int TotalPages { get; set; }
    public List<Subject> Subjects { get; set; } = new();
}

public class CreateSubjectViewModel
{
    [Required(ErrorMessage = "Please enter subject code")]
    [StringLength(10, ErrorMessage = "The code must be at least {2} characters long.", MinimumLength = 3)]
    [RegularExpression(@"^\S*$", ErrorMessage = "The code cannot contain whitespace.")]
    public string Code { get; set; }

    [Required(ErrorMessage = "Please enter subject name")]
    public string Name { get; set; }

    [Display(Name = "Activate")]
    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Please select a manager")]
    [Display(Name = "Manager")]
    public int SubjectManagerId { get; set; }
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
        [CustomAuthorize]
        public IActionResult Index(SubjectListViewModel vm)
        {
            var subjects = _context.Subjects.Include(subject => subject.SubjectManager).AsQueryable();

            if (!string.IsNullOrEmpty(vm.Search?.Trim()))
            {
                subjects = subjects.Where(subjects => subjects.Code.ToLower().Contains(vm.Search.Trim().ToLower()) || subjects.Name.ToLower().Contains(vm.Search.Trim().ToLower()));
            }

            switch (vm.Type)
            {
                case "active":
                    subjects = subjects.Where(subject => subject.IsActive == true);
                    break;
                case "deactive":
                    subjects = subjects.Where(subject => subject.IsActive == false);
                    break;
            }

            subjects = subjects.OrderByDescending(subject => subject.CreatedAt);

            int pageIndex = vm.PageIndex == 0 ? 1 : vm.PageIndex;
            int pageSize = 10;
            int itemCount = subjects.Count();
            int totalPages = (int)Math.Ceiling((double)itemCount / pageSize);

            subjects = subjects.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            vm.PageIndex = pageIndex;
            vm.PageSize = pageSize;
            vm.ItemCount = itemCount;
            vm.TotalPages = totalPages;
            vm.Subjects = subjects.ToList();

            ViewBag.SubjectManagers = _context.Users.Where(user => user.Role.Value == "Manager").ToList();

            return View(vm);
        }



        public IActionResult ProjectExist(string code)
        {
            var exist = _context.Subjects.Any(subject => subject.Code.ToLower() == code.Trim().ToLower());
            return Json(new { exist, message = "Project code existed" });
        }

        [Route("/subjects")]
        [HttpPost]
        public async Task<IActionResult> AddSubject(CreateSubjectViewModel vm, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                _context.Subjects.Add(new()
                {
                    Code = vm.Code.Trim().ToUpper(),
                    Name = vm.Name.Trim(),
                    IsActive = vm.IsActive,
                    SubjectManagerId = vm.SubjectManagerId
                });
                await _context.SaveChangesAsync();
                errorHelper.Success = "New subject successfully";
            }
            catch
            {
                errorHelper.Error = "New subject fail";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Active(int subjectId, int? PageIndex, string? Search, string? Type, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                var subject = _context.Subjects.SingleOrDefault(s => s.Id == subjectId);
                if (subject == null) return RedirectToAction("Index", new { PageIndex, Search, Type });
                subject.IsActive = !subject.IsActive;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { PageIndex, Search, Type });
            }
            catch
            {
                errorHelper.Error = "Something error";
                return RedirectToAction("Index", new { PageIndex, Search, Type });
            }
        }

        [Route("/subjects/{code}/information")]
        public IActionResult SubjectInformation(string code)
        {
            try
            {
                var subject = _context.Subjects.Include(s => s.SubjectManager).SingleOrDefault(s => s.Code == code);
                ViewBag.SubjectManagers = new SelectList(_context.Users.Where(user => user.Role.Value == RoleUser.Manager).ToList(), "Id", "Name");
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
            catch
            {
                return RedirectToAction("Index", new { code = code });
            }
        }

        [Route("/subjects/{code}/information")]
        [HttpPost]
        public async Task<IActionResult> SubjectInformation(AddSubjectViewModel vm, string code, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                ViewBag.SubjectManagers = new SelectList(_context.Users.Where(user => user.Role.Value == RoleUser.Manager).ToList(), "Id", "Name");
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
            catch
            {
                errorHelper.Error = "Something error";
                return View(vm);
            }
        }

        [Route("/subjects/{code}/assignments")]
        public IActionResult Assignments(string code, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
              var assignments = _context.Assignments.Where(ass => ass.Subject.Code == code);
                return View(new AssignmentViewModel() { Assignments = assignments.ToList() });
            }
            catch
            {
                errorHelper.Error = "Something error";
                return RedirectToAction("NotFound", "Error");
            }
        }


        //WARMING: DELETING ASSIGNMENT
        public async Task<IActionResult> AssignmentsActive(string code, int assignmentId, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                var assignment = _context.Assignments.SingleOrDefault(s => s.Id == assignmentId);
                if (assignment == null) return RedirectToAction("Assignments");
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
                errorHelper.Success = "Delete successfully";
                return RedirectToAction("Assignments", new { code });
            }
            catch
            {
                errorHelper.Error = "Update fail, something error";
                return RedirectToAction("Assignments", new { code });
            }
        }

        [Route("/subjects/{code}/assignments")]
        [HttpPost]
        public async Task<IActionResult> Assignments(AssignmentViewModel vm, string code, [FromServices] ErrorHelper errorHelper)
        {

            try
            {
                var subject = _context.Subjects.SingleOrDefault(s => s.Code == code);
                if (subject == null) return NotFound();

                Assignment assignment = new Assignment()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Weight = vm.Weight,
                    SubjectId = subject.Id
                };
                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();
                errorHelper.Success = "Add assignment successfully";
            }
            catch
            {
                errorHelper.Error = "Something error";
            }
            return RedirectToAction("Assignments", new { code });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssignment(AssignmentViewModel2 vm, string code, [FromServices] ErrorHelper errorHelper)
        {
            try
            {
                var assignment = _context.Assignments.SingleOrDefault(ass => ass.Id == vm.Id);
                if (assignment == null) return RedirectToAction("Assignments", new { code });

                assignment.Name = vm.Name;
                assignment.Description = vm.Description;
                assignment.Weight = vm.Weight;
                errorHelper.Success = "Update assignment successfully";
                await _context.SaveChangesAsync();
            }
            catch
            {
                errorHelper.Error = "Something error";
            }

            return RedirectToAction("Assignments", new { code });
        }
    }
}