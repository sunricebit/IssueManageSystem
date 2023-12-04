using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.Controllers
{
    public class LandingController : Controller
    {
        private readonly IMSContext context;

        public LandingController(IMSContext context)
        {
            this.context = context;
        }
        [Route("/landing")]
        public async Task<IActionResult> Index()
        {
            //using (IMSContext context = new IMSContext())
            //{
               var type= context.Settings.ToList();
                //ViewBag.Types = type;
                ViewBag.Setting = new SelectList(context.Settings, "Id", "Value");
                return View();
            //}
        }

        [Route("/landing")]
        [HttpPost]
        public async Task<IActionResult> Index(IMS.ViewModels.Auth.Contact _contact)
        {
                ViewBag.Setting = new SelectList(context.Settings, "Id", "Value");
            
            try
            {
                if (_contact.Id == 0)
                {
                    ViewBag.Success = "error";
                    ViewBag.Error = "Please choose a Contact type";
                    return View(nameof(Index));
                }
                if (ModelState.IsValid)
                {
                    using (IMSContext context = new IMSContext())
                    {
                        var contact = new Contact
                        {
                            Name = _contact.Name,
                            Email = _contact.Email,
                            Phone = _contact.Phone,
                            ContactTypeId = _contact.Id,
                        };
                        context.Contacts.Add(contact);
                        context.SaveChanges();// Lưu để có ContactId
                        var message = new Message
                        {
                            ContactId = contact.Id,
                            Content = _contact.Content,
                        };
                        context.Messages.Add(message);
                        context.SaveChanges();
                        ViewBag.Success = "Send contact success!";
                        ModelState.Clear();
                        return View(nameof(Index));
                    }
                }
                else
                {
                    ViewBag.Success = "error";
                    return View(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return View();
            }
           
        }

    }
}
