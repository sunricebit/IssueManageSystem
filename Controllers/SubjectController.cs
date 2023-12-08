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

    [Required]
    [StringLength(10, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 3)]
    public string? Code { get; set; }

    [Required]
    public string? Name { get; set; }


    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = false;

    [Required]
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
        public IActionResult Index(int? page, string? search, string? type)
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

            subjects = subjects.Include(subject => subject.SubjectManager).OrderByDescending(s=> s.CreatedAt);
            ViewBag.SubjectManagers = new SelectList( _context.Users.Where(user => user.Role.Value == RoleUser.SubjectManager).ToList(),  "Id", "Name");

            PaginateEnginee<Subject, SubjectSearchViewModel> a = PaginateEnginee<Subject, SubjectSearchViewModel>.Create(subjects, page ?? 1);
            a.Additional = new SubjectSearchViewModel(search ?? "", type ?? "");

            return View(a);
        }

        [Route("/subjects")]
        [HttpPost]
        public IActionResult Index(IFormCollection collection,  int? page, string? search, string? type)
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
            PaginateEnginee<Subject, SubjectSearchViewModel> a = PaginateEnginee<Subject, SubjectSearchViewModel>.Create(subjects, page ?? 1);
            a.Additional = new SubjectSearchViewModel(search ?? "", type ?? "");
            return View(a);
        }



        public async Task<IActionResult> Active(int subjectId, int? page, string? search, string? type)
        {
            var subject = _context.Subjects.SingleOrDefault(s => s.Id == subjectId);
            if(subject == null) return RedirectToAction("Index", new { page= page, search = search, type= type });
            subject.IsActive = !subject.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { page = page, search = search, type = type });
        }
    }
}