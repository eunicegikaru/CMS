using Dapper;
using DBL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;

namespace DBL.Repositories
{
    public class ClientsRepository : BaseRepository, IClientsRepository
    {

        private readonly string _connectionstring;


        public ClientsRepository(string DefaultConnection) : base(DefaultConnection)
        {
            _connectionstring = DefaultConnection ?? throw new ArgumentNullException(nameof(DefaultConnection));
        }


        public async Task CreateUser(Users user)
        {
            using var conn = new SqlConnection(_connectionstring);
            await conn.ExecuteAsync(
                "sp_CreateUser",
                new
                {
                    user.FullName,
                    user.Email,
                    user.PasswordHash,
                    user.Role
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Users?> GetByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionstring);
            return await conn.QueryFirstOrDefaultAsync<Users>(
                "sp_GetUserByEmail",
                new { Email = email },
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateLoginStats(int userId)
        {
            var sql = @"
        UPDATE Users
        SET 
            LastLoginAt = GETDATE(),
            LoginCount = ISNULL(LoginCount, 0) + 1
        WHERE Id = @Id";

            using var conn = new SqlConnection(_connectionstring);
            await conn.ExecuteAsync(sql, new { Id = userId });
        }
       


    }
}
