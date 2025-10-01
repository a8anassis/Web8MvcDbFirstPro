using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.DTO;
using SchoolApp.Services;
using System.Security.Claims;

namespace SchoolApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IApplicationService applicationService;

        public UserController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal? principal = HttpContext.User;

            if (!principal!.Identity!.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");   // Dashboard todo move to dashboard
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO credentials)
        {
            try
            {
                var user = await applicationService.UserService.VerifyAndGetUserAsync(credentials);

                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (user == null)
                {
                    ViewData["ValidateMessage"] = "Bad Credentials. Username or Password is invalid.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()!)
                };


                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new()
                {
                    AllowRefresh = true,
                    IsPersistent = credentials.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);


                // Redirect based on role
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
    }
}



// Check roles and redirect accordingly
//if (User.IsInRole("Admin"))
//{
//    return RedirectToAction("Index", "Admin");
//}
//else if (User.IsInRole("Teacher"))
//{
//    return RedirectToAction("Index", "Teacher");
//}
//else if (User.IsInRole("Student"))
//{
//    return RedirectToAction("Index", "Student");
//}
//else
//{
//    return RedirectToAction("Index", "User");
//}
