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
        public async Task<IActionResult> List()
        {
             var contactMessages = await _context.Contacts.ToListAsync();
          //  ViewBag.ContactMessages = contactMessages;
            return View(contactMessages);
        }

        [Route("MaskValid")]
        public IActionResult MarkValid(int id)
        {
            Contact contact = _context.Contacts.SingleOrDefault(n => n.Id == id);
            contact.IsValid = !contact.IsValid;
            return View();
        }

    }
}
