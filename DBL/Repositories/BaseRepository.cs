// DBL/Repositories/BaseRepository.cs
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DBL.Repositories
{
    public abstract class BaseRepository
    {
        private readonly string _connectionstring;

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
    }
}
