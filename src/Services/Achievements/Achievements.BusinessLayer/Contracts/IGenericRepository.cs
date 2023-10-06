namespace Achievements.BusinessLayer.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAsync(int range);
        Task<T?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
    }
}
