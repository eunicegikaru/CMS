// Controllers/ManagerController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult Task()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Analytics()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

    }
}