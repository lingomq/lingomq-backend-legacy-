namespace Topics.DataAccess.Dapper.Contracts;
public interface IGenericRepository<T>
{
    Task AddAsync(T entity);
    Task<List<T>> GetAsync(int range = int.MaxValue);
    Task<T?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(T entity);
}
