
// Controllers/ClientController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }
        public IActionResult MyProject()
        {
            return View();
        }
        public IActionResult Invoices()
        {
            return View();
        }
        public IActionResult Messages()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Documents()
        {
            return View();
        }
        public IActionResult MyProfile()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}