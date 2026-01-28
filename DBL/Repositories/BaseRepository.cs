using System;
using System.Data;


namespace DBL.Repositories
{
    public class BaseRepository
    {
        private readonly string _defaultConnection;

        public BaseRepository(string defaultConnection)
        {
            _defaultConnection = defaultConnection;
        }

        public string GetAllStatement(string tableName)
        {
            return $"SELECT * FROM {tableName}";
        }

        protected IDbConnection CreateConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection (_defaultConnection);
        }
    }
}
