using AspNetCoreGeneratedDocument;
using CMS.ViewModels;
using DBL;
using DBL.Models;
using DBL.Repositories;
using DBL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly Bl _bl;
        private readonly ILogServices _log;
        private readonly ClientsRepository _repo;

        public AuthController(ClientsRepository repo,  ILogServices log)
        {
            _bl = new Bl();
            _log = log;
            _repo = repo;
        }
        [HttpGet]
        [AllowAnonymous]
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
                Role = model.Role
            };

            await _repo.CreateUser(user);
            return user.Role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Manager" => RedirectToAction("Dashboard", "Manager"),
                _ => RedirectToAction("Dashboard", "Client")
            };
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

            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            // ✅ UPDATE DATABASE HERE
            await _repo.UpdateLoginStats(user.Id);

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

            return user.Role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Manager" => RedirectToAction("Dashboard", "Manager"),
                _ => RedirectToAction("Dashboard", "Client")
            };
        }
        public static class PasswordHelper
        {
            public static string HashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            public static bool VerifyPassword(string password, string hash)
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
        }

    }
}
