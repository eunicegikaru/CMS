using CMS.ViewModels;
using DBL.Models;
using DBL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CMS.Controllers.AuthController;

public class AccountController : Controller
{
    private readonly ClientsRepository _repo;

    public AccountController(ClientsRepository repo)
    {
        _repo = repo;

    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existing = await _repo.GetByEmail(model.Email);
        if (existing != null)
        {
            ModelState.AddModelError("", "Email already exists");
            return View(model);
        }

        var user = new Users
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = PasswordHelper.HashPassword(model.Password),
            Role = "User"
        };

        await _repo.CreateUser(user);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _repo.GetByEmail(model.Email);

        if (user == null || user.PasswordHash != PasswordHelper.HashPassword(model.Password))
        {
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "MyCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("MyCookie", principal, new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = DateTime.UtcNow.AddDays(7)
        });

        return RedirectToAction("Dashboard", "Home");
    }
}
