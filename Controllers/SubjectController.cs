using Microsoft.AspNetCore.Mvc;


public class SubjectSearchViewModel
{
    public string Search { get; set; }
    public string Type { get; set; }

    public SubjectSearchViewModel(string search, string type)
    {
        Search = search;
        Type = type;
    }
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


            PaginateEnginee<Subject, SubjectSearchViewModel> a = PaginateEnginee<Subject, SubjectSearchViewModel>.Create(subjects, page ?? 1);
            a.Additional = new SubjectSearchViewModel(search ?? "", type ?? "");

            return View(a);
        }

        [Route("/subjects")]
        public IActionResult Active(int? page, string? search, string? type)
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


            PaginateEnginee<Subject, SubjectSearchViewModel> a = PaginateEnginee<Subject, SubjectSearchViewModel>.Create(subjects, page ?? 1);
            a.Additional = new SubjectSearchViewModel(search ?? "", type ?? "");

            return View(a);
        }
    }
}