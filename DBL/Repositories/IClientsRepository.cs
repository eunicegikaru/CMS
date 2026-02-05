using DBL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Repositories
{
    public interface IClientsRepository
    {
        Task CreateUser(Users user);
        Task<Users?> GetByEmail(string email);
        Task UpdateLoginStats(int userId);
    }
}
