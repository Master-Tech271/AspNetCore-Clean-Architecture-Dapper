namespace Api.Application.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> AddAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByColumnAsync(string columnName, object value);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}