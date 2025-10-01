using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.DTO;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IApplicationService applicationService;
        public List<Error> ErrorArray { get; set; } = [];


        public TeacherController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(TeacherSignupDTO teacherSignupDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(teacherSignupDTO);
            }
            
            try
            {
                await applicationService.TeacherService.SignUpUserAsync(teacherSignupDTO);
                return RedirectToAction("Login", "User");
            }
            catch (Exception e)
            {
                ErrorArray.Add(new Error("", e.Message, ""));
                ViewData["ErrorArray"] = ErrorArray;
                return View();
            }
        }
    }
}
