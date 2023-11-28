using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [Route("/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IMSContext _context;

        public AuthController(IMSContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }


        [Route("sign-up")]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [Route("sign-up")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel vm, [FromServices] IMailService mailService, [FromServices] IHashService hashService)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = _context.Users.FirstOrDefault(user => user.Email == vm.Email);
            if(user != null )
            {
                ViewBag.Error = "Email already registered. Try another one";
                return View(vm);
            }

            var role = _context.Settings.FirstOrDefault(s => s.Type == "ROLE" && s.Value == "Student");
            if (role == null)
            {
                return View(vm);
            };

            try
            {
                User userCreate = new User
                {
                    Email = vm.Email.Trim(),
                    Name = mailService.GetAddress(vm.Email)!,
                    Password = hashService.HashPassword(vm.Password),
                    ConfirmToken = hashService.RandomHash(),
                    RoleId = role.Id
                };
                _context.Users.Add(userCreate);
                await _context.SaveChangesAsync();
                mailService.SendMailConfirm(userCreate.Email, userCreate.ConfirmToken!);
                ViewBag.Success = "Success! Your registration is complete. Check your email for confirmation";
                return View(new SignUpViewModel());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Something error";
            }

            return View(new SignUpViewModel());

        }

        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }
    }
}