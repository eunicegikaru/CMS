using Dapper;
using DBL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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

        //public void TestConnection()
        //{

        //    try
        //    {
        //        using var conn = new SqlConnection(_connectionstring);
        //        conn.Open();
        //        Console.WriteLine("✅ Database connected successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("❌ Connection failed: " + ex.Message);
        //    }
        //}
        public async Task CreateUser(Users user)
        {
            using var db = new SqlConnection(_connectionstring);
            string sql = @"INSERT INTO Users (FullName, Email, PasswordHash, Role)
                       VALUES (@FullName, @Email, @PasswordHash, @Role)";
            await db.ExecuteAsync(sql, user);
        }

        public async Task<Users?> GetByEmail(string email)
        {
            using var db = new SqlConnection(_connectionstring);
            return await db.QueryFirstOrDefaultAsync<Users>(
                "SELECT * FROM Users WHERE Email = @Email",
                new { Email = email });
        }
    }
}
