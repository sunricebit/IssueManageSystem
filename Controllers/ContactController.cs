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

        public IActionResult Index()
        {
            return View();
        }
        [Route("Lists")]
        public IActionResult List()
        {
            var contactMessages = _context.Contacts.ToListAsync();
            ViewBag.ContactMessages = contactMessages;
            return View("~/Views/Contact/List.cshtml");
        }

        public IActionResult MarkValid(int id)
        {
            Contact contact = _context.Contacts.SingleOrDefault(n => n.Id == id);
            contact.IsValid = !contact.IsValid;
            return List();
        }

    }
}
