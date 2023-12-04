using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("Contact")]
    public class ContactController : Controller
    {
        private readonly IMSContext _context;
        private readonly IMailService _mailService;

        public ContactController(IMSContext context,IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
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
            _context.SaveChanges();
            return RedirectToAction("List", "Contact");
        }
        [Route("/detail")]
        public IActionResult Details(int id)
        {
            Contact contact = _context.Contacts.Include(n => n.ContactHandlings).SingleOrDefault(n => n.Id == id);
            return View(contact);
        }

        [Route("/add-note")]
        public IActionResult AddNote(int contactID,string note)
        {
            Contact contact = _context.Contacts.Include(n => n.ContactHandlings).SingleOrDefault(n => n.Id == contactID);
            ContactHandling contactHandling = new ContactHandling()
            {
                Contact = contact,
                Description = "Add Note To Contact #" + contact.Id,
                Note = note,
                CreatedDate = DateTime.Now
            };
            _context.Add(contactHandling);
            _context.SaveChanges();
            return RedirectToAction("Details", "Contact", new {id = contact.Id});
        }

        [Route("/update-note")]
        public IActionResult UpdateNote(int id, string note)
        {
            ContactHandling contactHandling = _context.ContactHandlings.SingleOrDefault(n => n.Id == id);
            contactHandling.Note = note;
            _context.SaveChanges();
            return RedirectToAction("Details", "Contact", new { id = contactHandling.ContactId });
        }

        [Route("/send-email")]
        public IActionResult sendMail(int id)
        {
           // ContactHandling contactHandling = _context.ContactHandlings.SingleOrDefault(n => n.Id == id);
           // _mailService.SendMailContact(contactHandling.Contact.Email,contactHandling.Note);

            return RedirectToAction("Details", "Contact", new { id = contactHandling.ContactId });
        }

    }
}
