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
            
            return RedirectToDashboard(principal);
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Usually the user ID
                    new Claim(ClaimTypes.Name, user.Username), // This sets User.Identity.Name
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
                //ClaimsPrincipal? principal = HttpContext.User;
                var principal = new ClaimsPrincipal(identity);
                return RedirectToDashboard(principal);
            }
            catch (Exception ex)
            {
                ViewData["ValidateMessage"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }


        private IActionResult RedirectToDashboard(ClaimsPrincipal user)
        {
            if (user.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            else if (user.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                return RedirectToAction("Index", "Home"); // Fallback
            }
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
