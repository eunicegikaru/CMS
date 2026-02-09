// DBL/Bl.cs
using DBL.Repositories;
using DBL.UOW;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DBL
{
    public class Bl
    {
        private string _connectionstring;
        private UnitOfWork _db;
        private readonly IConfiguration _configuration;

        public ClientsRepository ClientRepository => (ClientsRepository)_db.ClientsRepository;

        public Bl(string connectionString, IConfiguration configuration)
        {
            _connectionstring = connectionString;
            _db = new UnitOfWork(_connectionstring);
            _configuration = configuration;
            _connectionstring = _configuration.GetConnectionString("DefaultConnection");
        }

        public Bl()
        {
            // Default constructor if needed
        }

        public Bl(string? connectionString)
        {
            _connectionstring = connectionString;
        }

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionstring);
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionstring = connectionString;
            if (_db == null)
            {
                _db = new UnitOfWork(_connectionstring);
            }
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionstring);
        public string GetConnectionString() => _connectionstring;
    }
}