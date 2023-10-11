namespace Finances.BusinessLayer.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetAsync(Guid id);
        Task<List<T?>> GetAsync(int range);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
