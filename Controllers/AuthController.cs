using IMS.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace IMS.Controllers
{
    [Route("/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IMSContext _context;

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri;


        public AuthController(IMSContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            clientId = Configuration["Authentication:Google:ClientId"];
            clientSecret = Configuration["Authentication:Google:ClientSecret"];
            redirectUri = Configuration["Authentication:Google:RedirectUri"];
        }

        [Route("google/callback")]
        public async Task<ActionResult> Google([FromQuery] string code, [FromServices] IMailService mailService, [FromServices] IHashService hashService)
        {
            string authorizationCode = code;

            var httpClient = new HttpClient();
            var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "code", authorizationCode },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            });
            var tokenResponse = await httpClient.PostAsync("https://accounts.google.com/o/oauth2/token", tokenRequest);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            GoogleInfo tokenInfo = JsonSerializer.Deserialize<GoogleInfo>(tokenContent)!;

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenInfo.access_token);
            var peopleResponse = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
            string mailContent = await peopleResponse.Content.ReadAsStringAsync();
            MailInfo mailInfo = JsonSerializer.Deserialize<MailInfo>(mailContent)!;
            string email = mailInfo.email;

            var user = _context.Users.FirstOrDefault(user => user.Email == email);
            if (user == null)
            {
                var role = _context.Settings.FirstOrDefault(s => s.Type == "ROLE" && s.Value == "Student");
                if (role == null)
                {
                    return View();
                };

                user = new User
                {
                    Email = email,
                    Password = hashService.HashPassword(hashService.RandomStringGenerator(8)),
                    Name = mailService.GetAddress(email)!,
                    Status = true,
                    RoleId = role.Id,
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

            }

            HttpContext.Session.SetUser(user);

            return Redirect("/");
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
                Password = hashService.HashPassword(vm.Password),
                Name = mailService.GetAddress(vm.Email)!,
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
            var user = _context.Users.Include(s => s.Role).FirstOrDefault(user => user.Email == vm.Email);

            if (user == null || !hashService.Verify(vm.Password, user.Password))
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

        [Route("logout")]
        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }

    }
}