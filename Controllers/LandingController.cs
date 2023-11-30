using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.IO;
using System.Text.Json;

namespace IMS.Controllers
{
    public class LandingController : Controller
    {
        [Route("/landing")]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }



        [HttpPost("sendcontact")]
        public async Task<IActionResult> SendContact(string name, string email,string phone,string message)
        {
            using (IMSContext context = new IMSContext())
            {
                var contact = new Contact
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Message = message,
                };
                context.Contacts.Add(contact);
                context.SaveChanges();
                return View(nameof(Index));
            }
               
            return View();
        }

    }
}
