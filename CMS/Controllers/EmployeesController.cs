using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    [Authorize(Roles = "Employees")]
    public class EmployeesController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
