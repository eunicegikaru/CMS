using DBL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Repositories
{
    public interface IClientsRepository
    {
        Task<(string? users, dynamic totalCount)> GetAllUsersByRole(string role, int page, int v);

        public interface IClientsRepository
        {
            Task<int> CreateUser(Users user);
            Task<Users?> GetByEmail(string email);
            Task<Users?> GetById(int userId);
            Task UpdateLoginStats(int userId);
            Task<bool> UpdateUser(UserUpdateDto userDto);
            Task<(IEnumerable<Users> users, int totalCount)> GetAllUsersByRole(string? role = null, int pageNumber = 1, int pageSize = 20);
        }
    }
}
