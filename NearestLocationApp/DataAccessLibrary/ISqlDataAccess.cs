
namespace DataAccessLibrary
{
    public interface ISqlDataAccess
    {
        Task<List<T>> GetData<T, U>(string sql, U parameters);
        Task SetData<U>(string sql, U parameters);
    }
}