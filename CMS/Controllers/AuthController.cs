using DBL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly ClientsRepository _repo;

        public AuthController(ClientsRepository repo)
        {
            _repo = repo;
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
