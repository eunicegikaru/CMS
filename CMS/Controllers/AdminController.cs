using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
    public IActionResult Employees()
    {
        return View();
    }
    public IActionResult projects()
    {
        return View();
    }
    public IActionResult invoices()
    {
        return View();
    }
    public IActionResult analytics()
    {
        return View();
    }
    public IActionResult Audits()
    {
        return View();
    }
}
