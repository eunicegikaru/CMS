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

        //[Route("api/employee")]
        //[ApiController]
        //public class EmployeeApiController : ControllerBase
        //{
        //    private readonly IEmployeesRepository _repo;

        //    public EmployeeApiController(IEmployeesRepository repo)
        //    {
        //        _repo = repo;
        //    }

        //    [HttpGet("getall")]
        //    public async Task<IActionResult> GetAll()
        //    {
        //        var employees = await _repo.GetAllEmployees();
        //        return Ok(employees);
        //    }
        //}

        public IActionResult Activities()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View();
        }
        public IActionResult Invoices()
        {
            return View();
        }
        public IActionResult Tasks()
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
    }
}