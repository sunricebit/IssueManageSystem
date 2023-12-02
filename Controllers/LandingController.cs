using DocumentFormat.OpenXml.InkML;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
namespace IMS.Controllers
{
    public class LandingController : Controller
    {
        [Route("/landing")]
        public async Task<IActionResult> Index()
        {
            using (IMSContext context = new IMSContext())
            {
               var type= context.Settings.ToList();
                ViewBag.Types = type;
                return View();
            }
        }


        [HttpPost("sendcontact")]
        public async Task<IActionResult> SendContact(Contact _contact, string Type)
        {
            try {
                if (Type == "Contact type *")
                {
                    using (IMSContext context = new IMSContext())
                    {
                        var types = context.Settings.ToList();
                        ViewBag.Types = types;
                        ViewBag.Success = "error";
                        ViewBag.Error = "Please choose a Contact Type.";
                        return View(nameof(Index));
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        using (IMSContext context = new IMSContext())
                        {
                            Console.WriteLine(Type);
                            var types = context.Settings.ToList();
                            ViewBag.Types = types;
                            var type = context.Settings.SingleOrDefault(t => t.Value == Type);

                            var contact = new Contact
                            {
                                Name = _contact.Name,
                                Email = _contact.Email,
                                Phone = _contact.Phone,
                                Reason = _contact.Reason,
                                Message = _contact.Message,
                                ContactTypeId = type?.Id,
                            };
                            context.Contacts.Add(contact);
                            context.SaveChanges();
                            ViewBag.Success = "Send contact success!";
                            return View(nameof(Index));
                        }
                    }
                    else
                    {
                        using (IMSContext context = new IMSContext())
                        {
                            var types = context.Settings.ToList();
                            ViewBag.Types = types;
                            ViewBag.Success = "error";
                            return View(nameof(Index));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" +ex.Message;
                return View();
            }
        }

    }
}
