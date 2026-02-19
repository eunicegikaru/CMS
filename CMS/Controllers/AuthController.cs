// CMS/Controllers/AuthController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using CMS.ViewModels;
using DBL;
using DBL.Models;
using DBL.Helpers;

namespace CMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly Bl _bl;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Initialize Bl with connection string from configuration
            var connectionstring = _configuration.GetConnectionString("DefaultConnection");
            _bl = new Bl(connectionstring);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Check if email already exists
                var existingUser = await _bl.ClientRepository.GetByEmail(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                // Validate password
                if (!PasswordHelper.IsValidPassword(model.Password))
                {
                    ModelState.AddModelError("Password", "Password must be at least 6 characters");
                    return View(model);
                }

                // Validate role
                if (!model.AvailableRoles.Contains(model.Role))
                {
                    ModelState.AddModelError("Role", "Invalid role selected");
                    return View(model);
                }

                // Create user
                var user = new Users
                {
                    FullName = model.FullName.Trim(),
                    Email = model.Email.Trim().ToLower(),
                    PasswordHash = PasswordHelper.HashPassword(model.Password),
                    Role = model.Role,
                    IsActive = true
                };

                var userId = await _bl.ClientRepository.CreateUser(user);

                if (userId > 0)
                {
                    _logger.LogInformation($"User registered successfully: {user.Email}, Role: {user.Role}");

                    // Optionally log user in immediately after registration
                    await SignInUser(user);

                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToDashboard(user.Role);
                }

                ModelState.AddModelError("", "Registration failed. Please try again.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Registration failed for email: {model.Email}");
                ModelState.AddModelError("", $"Registration failed: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Get user by email
                var user = await _bl.ClientRepository.GetByEmail(model.Email);

                // Validate user
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                if (!user.IsActive)
                {
                    ModelState.AddModelError("", "Account is deactivated. Please contact administrator.");
                    return View(model);
                }

                // Verify password
                if (!PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                // Update login statistics
                await _bl.ClientRepository.UpdateLoginStats(user.Id);

                // Create claims and sign in
                await SignInUser(user, model.RememberMe);

                _logger.LogInformation($"User logged in: {user.Email}, Role: {user.Role}");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToDashboard(user.Role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login failed for email: {model.Email}");
                ModelState.AddModelError("", "Login failed. Please try again.");
                return View(model);
            }
        }

        private async Task SignInUser(Users user, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("LastLogin", user.LastLogin?.ToString() ?? DateTime.UtcNow.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private IActionResult RedirectToDashboard(string role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Manager" => RedirectToAction("Dashboard", "Manager"),
                "Client" => RedirectToAction("Dashboard", "Client"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}