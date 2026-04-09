using Abp.Threading;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace ShoppingAPI.Persistence
{        
    public class ReadOnlyData : IReadOnlyData  
    {
        private readonly string _connectionString;

        public ReadOnlyData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default") ?? string.Empty;
        }

        public T? GetEntity<T>(string sql, object? parameters) { 
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.QueryFirstOrDefault<T>(sql, parameters);
        }

        public List<T> GetEntityList<T>(string sql, object? parameters)
        {
            return AsyncHelper.RunSync(() => GetEntityListAsync<T>(sql, parameters));
        }

        public async Task<List<T>> GetEntityListAsync<T>(string sql, object? parameters)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var result = await db.QueryAsync<T>(sql, parameters);
            return result.ToList();
        }
    }
}
