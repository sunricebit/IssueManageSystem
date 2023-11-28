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
            if (!ModelState.IsValid) return View();

            var user = _context.Users.FirstOrDefault(user => user.Email == vm.Email);
            if (user != null)
            {
                ViewBag.Error = "Email already registered. Try another one";
                return View();
            }

            var role = _context.Settings.FirstOrDefault(s => s.Type == "ROLE" && s.Value == "Student");
            if (role == null)
            {
                return View();
            };

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
            ModelState.Clear();
            return View(new SignUpViewModel());
        }


        [Route("confirm/{token}")]
        public async Task<IActionResult> Confirm(string token)
        {
            User? user = _context.Users.FirstOrDefault(user => user.ConfirmToken == token);

            if (user == null)
            {
                ViewBag.AlertMessage = "Token invalid";
                return RedirectToAction("SignIn");
            }

            user.ConfirmToken = null;
            user.Status = true;

            await _context.SaveChangesAsync();
            return RedirectToAction("SignIn"); ;
        }

        [Route("sign-in")]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }

        [Route("sign-in")]
        [HttpPost]
        public IActionResult SignIn(SignInViewModel vm, [FromServices] IHashService hashService)
        {
            if (!ModelState.IsValid) return View();
            var user = _context.Users.Include(s => s.Role).FirstOrDefault(p => p.Email == vm.Email);

            if (user == null)
            {
                ViewBag.Error = "Email or password incorrect!";
                return View();
            }

            bool validPass = hashService.Verify(vm.Password, user.Password);
            if (!validPass)
            {
                ViewBag.Error = "Email or password incorrect!";
                return View();
            }

            if (user.Status == null)
            {
                ViewBag.Error = "This account has not been verified yet!";
                return View();
            }

            if (user.Status == false)
            {
                ViewBag.Error = "This account has been locked!";
                return View();
            }

            HttpContext.Session.SetUser(user);

            ModelState.Clear();
            return Redirect("/");
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }


        [Route("forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm, [FromServices] IMailService mailService, [FromServices] IHashService hashService)
        {
            if (!ModelState.IsValid) return View();

            User? user = _context.Users.FirstOrDefault(user => user.Email == vm.Email);
            if (user == null)
            {
                ViewBag.Error = "This account does not exist";
                return View();
            }

            string hash = hashService.RandomHash();
            user.ResetToken = hash;
            await _context.SaveChangesAsync();
            mailService.SendResetPassword(vm.Email, hash);
            ViewBag.Success = "Check your email for a password reset token";
            ModelState.Clear();
            return View();
        }

        [Route("reset-password/{token}")]
        public IActionResult ResetPassword()
        {
            return View(new ResetPasswordViewModel());
        }

        [Route("reset-password/{token}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm, [FromServices] IHashService hashService, string token)
        {
            if (!ModelState.IsValid) return View();

            User? user = _context.Users.FirstOrDefault(user => user.ResetToken == token);
            if (user == null)
            {
                ViewBag.Error = "This token is not valid!!";
                return View();
            }

            user.ResetToken = null;
            user.Password = hashService.HashPassword(vm.Password);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Password has been reset successfully!!";
            ModelState.Clear();
            return View();
        }
    }
}