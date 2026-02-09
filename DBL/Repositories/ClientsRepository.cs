// DBL/Repositories/ClientsRepository.cs
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using DBL.Models;


namespace DBL.Repositories
{
   
    public class ClientsRepository : BaseRepository, IClientsRepository
    {
        private readonly Bl context;
        private string connectionstring;

        public ClientsRepository(Bl context) : base(context)
        {
            this.context = context;
        }

        public ClientsRepository(string connectionstring)
        {
            this.connectionstring = connectionstring;
        }

        public async Task<int> CreateUser(Users user)
        {
            using var connection = context.CreateConnection() as SqlConnection;
            if (connection == null) throw new InvalidOperationException("Connection is not SqlConnection");

            var parameters = new DynamicParameters();
            parameters.Add("@FullName", user.FullName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@Role", user.Role);
            parameters.Add("@UserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                await connection.ExecuteAsync("sp_CreateUser",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var userId = parameters.Get<int>("@UserId");
                return userId;
            }
            catch (SqlException ex) when (ex.Number == 51000)
            {
                throw new Exception("Email already exists", ex);
            }
            catch (SqlException ex) when (ex.Number == 51001)
            {
                throw new Exception("Invalid role specified", ex);
            }
        }

        public async Task<Users> GetByEmail(string email)
        {
            var parameters = new { Email = email };
            return await QueryFirstOrDefaultAsync<Users>("sp_GetUserByEmail", parameters);
        }

        public async Task<Users> GetById(int userId)
        {
            var parameters = new { UserId = userId };
            return await QueryFirstOrDefaultAsync<Users>("sp_GetUserById", parameters);
        }

        public async Task UpdateLoginStats(int userId)
        {
            var parameters = new { UserId = userId };
            await ExecuteAsync("sp_UpdateLoginStats", parameters);
        }

        public async Task<bool> UpdateUser(UserUpdateDto userDto)
        {
            var parameters = new
            {
                UserId = userDto.Id,
                FullName = userDto.FullName,
                Role = userDto.Role,
                IsActive = userDto.IsActive
            };

            var rowsAffected = await ExecuteScalarAsync<int>("sp_UpdateUser", parameters);
            return rowsAffected > 0;
        }

        public async Task<(IEnumerable<Users> users, int totalCount)> GetAllUsersByRole(
            string role = null,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var parameters = new
            {
                Role = string.IsNullOrEmpty(role) ? (object)null : role,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            using var multi = await QueryMultipleAsync("sp_GetAllUsersByRole", parameters);

            var users = await multi.ReadAsync<Users>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return (users, totalCount);
        }

        Task<(string? users, dynamic totalCount)> IClientsRepository.GetAllUsersByRole(string role, int page, int v)
        {
            throw new NotImplementedException();
        }
    }
}