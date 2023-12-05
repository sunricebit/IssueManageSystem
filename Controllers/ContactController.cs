using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using IMS.Models;
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
            var pageSize = 10;
            var pageIndex =  page ?? 1;
            var itemCount = _context.Contacts.Count();
            var totalPage = (int)Math.Ceiling((double)itemCount / pageSize);
            var data = _context.Contacts.Include(s=>s.ContactType).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.totalPage = totalPage;
            ViewBag.pageNum = pageIndex;         
            return View(data);
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
            Contact contact = _context.Contacts.Include(n => n.Messages).SingleOrDefault(n => n.Id == id);
            return View(contact);
        }

        [Route("/add-note")]
        public IActionResult AddNote(int contactID,string note)
        {
            Contact contact = _context.Contacts.Include(n => n.Messages).SingleOrDefault(n => n.Id == contactID);
            Message contactHandling = new Message()
            {
              Content = note , ContactId = contactID
            };
            _context.Add(contactHandling);
            _context.SaveChanges();
            return RedirectToAction("Details", "Contact", new {id = contact.Id});
        }

        [Route("/update-note")]
        public IActionResult UpdateNote(int id, string note)
        {
            Message message = _context.Messages.SingleOrDefault(n => n.Id == id);
            message.Content = note;
            _context.SaveChanges();
            return RedirectToAction("Details", "Contact", new { id = message.ContactId });
        }

        [Route("/send-email")]
        public IActionResult sendMail(int id)
        {
            Message contactHandling = _context.Messages.Include(n => n.Contact).SingleOrDefault(n => n.Id == id);
            _mailService.SendMailContact(contactHandling.Contact.Email,contactHandling.Content);

            return RedirectToAction("Details", "Contact", new { id = contactHandling.ContactId });
        }

    }
}
