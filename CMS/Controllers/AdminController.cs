// Controllers/AdminController.cs
using DBL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IClientsRepository _clientsRepo;
        private readonly IInvoicesRepository _invoicesRepo;
        private readonly IEmployeesRepository _employeesRepo;

        public AdminController(IClientsRepository clientsRepo, IInvoicesRepository invoicesRepo, IEmployeesRepository employeesRepo)
        {
            _clientsRepo = clientsRepo;
            _invoicesRepo = invoicesRepo;
            _employeesRepo = employeesRepo;
        }

        
        
        public IActionResult Activities()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View();
        }

        public async Task<IActionResult> Invoices()
        {
            // Load invoices to allow server-side rendering/fallback for the invoices table
            var invoices = await _invoicesRepo.GetAllInvoices();
            return View(invoices);
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // gather basic dashboard stats
            var employeeCount = await _employeesRepo.GetEmployeeCount();
            var activeProjects = await _employeesRepo.GetActiveProjectsCount();
            var tasksCompleted = await _employeesRepo.GetCompletedTasksCount();
            var pendingInvoices = await _invoicesRepo.GetPendingInvoicesCount();

            var model = new CMS.ViewModels.DashboardViewModel
            {
                EmployeeCount = employeeCount,
                ActiveProjects = activeProjects,
                TasksCompleted = tasksCompleted,
                PendingInvoices = pendingInvoices,
                TaskCompletionRate = 0 // kept for client-side calculation or future enhancement
            };

            return View(model);
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