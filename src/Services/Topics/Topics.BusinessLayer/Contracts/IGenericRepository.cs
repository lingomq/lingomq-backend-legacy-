namespace Topics.BusinessLayer.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAsync(int range);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
