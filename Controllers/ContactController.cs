using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Contact")]
    public class ContactController : Controller
    {
        private readonly IMSContext _context;

        public ContactController(IMSContext context)
        {
            _context = context;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Lists")]
        public IActionResult List(int? page)
        {
            int pageIndex = page != null ? page.Value : 1;
            var contactMessages = _context.Contacts.ToList();
            ViewBag.totalPage = contactMessages.Count % 5 == 0 ? (contactMessages.Count / 5) : (contactMessages.Count / 5 + 1);
            ViewBag.pageNum = pageIndex;
         
            return View(contactMessages.Skip(pageIndex - 1).Take(5));
        }

        [Route("MaskValid")]
        public IActionResult MarkValid(int id)
        {
            Contact contact = _context.Contacts.SingleOrDefault(n => n.Id == id);
            contact.IsValid = !contact.IsValid;
            return List(null);
        }

    }
}
