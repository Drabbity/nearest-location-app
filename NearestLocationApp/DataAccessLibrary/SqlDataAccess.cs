using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DataAccessLibrary
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public string ConnectionStringName { get; set; } = "DefaultSqlConnectionStringName";

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> GetData<T, U>(string sql, U parameters)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionStringName));

            var data = await connection.QueryAsync<T>(sql, parameters);
            return data.ToList();
        }

        public async Task SetData<U>(string sql, U parameters)
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionStringName));

            await connection.ExecuteAsync(sql, parameters);
        }
    }
}