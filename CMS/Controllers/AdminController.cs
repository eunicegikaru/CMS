// Controllers/AdminController.cs
using DBL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IClientsRepository _clientsRepo;

        public AdminController(IClientsRepository clientsRepo)
        {
            _clientsRepo = clientsRepo;
        }

        public IActionResult Dashboard()
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return View();
        }

        public async Task<IActionResult> Users(string role = null, int page = 1)
        {
            var (users, totalCount) = await _clientsRepo.GetAllUsersByRole(role, page, 20);
            ViewBag.TotalCount = totalCount;
            ViewBag.CurrentPage = page;
            ViewBag.SelectedRole = role;
            return View(users);
        }
    }
}