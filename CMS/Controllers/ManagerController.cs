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

        public IActionResult Task()
        {
            return View();
        }
    }
}