// DBL/Repositories/BaseRepository.cs
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DBL.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionstring;

        protected BaseRepository(string connectionstring)
        {
            _connectionstring = connectionstring
                ?? throw new ArgumentNullException(nameof(connectionstring));
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionstring);
        }

        protected async Task<T> ExecuteScalarAsync<T>(string storedProcedure, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        protected async Task<int> ExecuteAsync(string storedProcedure, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        protected async Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(string storedProcedure, object? parameters = null)
        {
            var connection = CreateConnection();
            return await connection.QueryMultipleAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<object>> GetMonthlyRevenue()
        {
            using var conn = new SqlConnection(_connectionstring);
            return await conn.QueryAsync(@"
        SELECT DATENAME(MONTH, DateCreated) AS month,
               SUM(Revenue) AS revenue,
               SUM(Expenses) AS expenses
        FROM Finance
        GROUP BY DATENAME(MONTH, DateCreated)");
        }
        public async Task<IEnumerable<object>> GetProjectStatus()
        {
            using var conn = new SqlConnection(_connectionstring);
            return await conn.QueryAsync(@"
        SELECT Status AS status, COUNT(*) AS count
        FROM Projects
        GROUP BY Status");
        }
        public async Task<IEnumerable<object>> GetRevenueByPeriod(string period)
        {
            using var conn = new SqlConnection(_connectionstring);

            string groupBy = period switch
            {
                "weekly" => "DATEPART(WEEK, DateCreated)",
                "yearly" => "YEAR(DateCreated)",
                _ => "DATENAME(MONTH, DateCreated)"
            };

            return await conn.QueryAsync($@"
        SELECT {groupBy} AS label,
               SUM(Revenue) AS revenue,
               SUM(Expenses) AS expenses
        FROM Finance
        GROUP BY {groupBy}");
        }

        public async Task<IEnumerable<object>> GetRecentActivity()
        {
            using var conn = new SqlConnection(_connectionstring);
            return await conn.QueryAsync(@"
        SELECT TOP 10 
            CONCAT(FirstName,' ',LastName) AS userName,
            'Logged in' AS action,
            Email AS details,
            LastLoginDate AS timestamp
        FROM Employees
        ORDER BY LastLoginDate DESC");
        }

    }
}
