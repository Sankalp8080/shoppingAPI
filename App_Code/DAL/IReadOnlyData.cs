using System.Data;

namespace ShoppingAPI.Persistence
{
    public interface IReadOnlyData
    { 
        Task<List<T>> GetEntityListAsync<T>(string sql, object? parameters); // Asynchronously get a list of entities based on the provided SQL query and parameters
        List<T> GetEntityList<T>(string sql, object? parameters); // Get a list of entities based on the provided SQL query and parameters
        T ?GetEntity<T>(string sql, object? parameters);    // Get a single entity based on the provided SQL query and parameters

    }
}
