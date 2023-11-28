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

            var role = _context.Settings.FirstOrDefault(s => s.Type == "ROLE" && s.Value == "Student");
            if(role ==  null) return View(vm);

            User user = new()
            {
                Email = vm.Email.Trim(),
                Name = mailService.GetAddress(vm.Email)!,
                Password = hashService.HashPassword(vm.Password),
                ConfirmToken = hashService.RandomHash(),
                RoleId = role.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            mailService.SendMailConfirm(user.Email, user.ConfirmToken);
            return View(new SignUpViewModel());
        }

        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }
    }
}