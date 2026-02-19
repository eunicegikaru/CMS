using Dapper;
using DBL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class EmployeesRepository : BaseRepository, IEmployeesRepository
    {
        public EmployeesRepository(string connectionstring) : base(connectionstring)
        {
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = "SELECT * FROM Employees ORDER BY CreatedAt DESC";

            return await connection.QueryAsync<Employee>(sql);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await connection.QueryFirstOrDefaultAsync<Employee>(
                "SELECT * FROM Employees WHERE Id=@Id", new { Id = id });
        }

        public async Task<int> CreateEmployee(Employee employee)
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = @"
        INSERT INTO Employees
        (EmployeeId, FirstName, LastName, Email, Phone, Role, Department, HireDate, IsActive, CreatedAt, UpdatedAt)
        VALUES
        (@EmployeeId, @FirstName, @LastName, @Email, @Phone, @Role, @Department, GETDATE(), 1, GETDATE(), GETDATE());

        SELECT CAST(SCOPE_IDENTITY() as int);";

            return await connection.ExecuteScalarAsync<int>(sql, employee);
        }

        public async Task<bool> UpdateEmployee(Employee employee)
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            string sql = @"
        UPDATE Employees SET
            FirstName=@FirstName,
            LastName=@LastName,
            Email=@Email,
            Phone=@Phone,
            Role=@Role,
            Department=@Department,
            IsActive=@IsActive,
            UpdatedAt=GETDATE()
        WHERE Id=@Id";

            return await connection.ExecuteAsync(sql, employee) > 0;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await connection.ExecuteAsync(
                "DELETE FROM Employees WHERE Id=@Id", new { Id = id }) > 0;
        }

        public async Task<IEnumerable<EmployeeActivity>> GetEmployeeActivity(int employeeId)
        {
            using var connection = CreateConnection() as SqlConnection;
            if (connection == null)
                throw new InvalidOperationException("Connection is not SqlConnection");

            return await connection.QueryAsync<EmployeeActivity>(
                @"SELECT * FROM EmployeeActivities
              WHERE EmployeeId=@EmployeeId
              ORDER BY Timestamp DESC",
                new { EmployeeId = employeeId });
        }
        public async Task<int> GetEmployeeCount()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null) throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Employees");
        }

        public async Task<int> GetActiveProjectsCount()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null) throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Projects WHERE Status='Active'");
        }

        public async Task<int> GetCompletedTasksCount()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null) throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Tasks WHERE Status='Completed'");
        }

        public async Task<int> GetPendingInvoicesCount()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null) throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Invoices WHERE Status='Pending'");
        }

        public async Task<IEnumerable<object>> GetEmployeePerformance()
        {
            using var conn = CreateConnection() as SqlConnection;
            if (conn == null) throw new InvalidOperationException("Connection is not SqlConnection");

            return await conn.QueryAsync(@"
        SELECT CONCAT(FirstName,' ',LastName) AS employeeName,
               COUNT(t.Id) AS tasksCompleted,
               AVG(t.Score) AS performanceScore
        FROM Employees e
        LEFT JOIN Tasks t ON e.Id = t.EmployeeId AND t.Status='Completed'
        GROUP BY FirstName, LastName");
        }

    }

}
