using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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


    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Please select a manager")]
    public int SubjectManagerId { get; set; } = 2;

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
                intermediate.Code = code;
                intermediate.Name = name;
                intermediate.Description = description;
                intermediate.SubjectManagerId = subjectManagerId;
                return RedirectToAction("Index", new { page = page, search = search, type = type });
            }

            Subject newSujnect = new()
            {
                Code = code,
                Name = name,
                Description = description,
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
    }
}