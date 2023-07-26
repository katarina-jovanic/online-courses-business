using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zavrsni.Models;

namespace Zavrsni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User modelLogin, Teacher teacher, Student student, Admin admin)
        {

            if (modelLogin.UserID == teacher.TeacherID || modelLogin.UserID == student.StudentID)
            {

            }

            return View();
        }
    }
}