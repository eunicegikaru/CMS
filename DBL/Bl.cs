using DBL.Repositories;
using DBL.UOW;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DBL
{
    public class Bl
    {
        string _connectionstring;
        UnitOfWork db;
        public ClientsRepository ClientRepository => (ClientsRepository)db.ClientsRepository;




        public Bl(string ConnectionString)
        {
            _connectionstring = ConnectionString;
            db = new UnitOfWork(_connectionstring);
        }

        public Bl()
        {
        }

        //public Bl()
        //{
        //}

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionstring);
        }
        public string Client()
        {
            return _connectionstring;
        }

        internal IDisposable CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}

