namespace Words.BusinessLayer.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAsync(int range = int.MaxValue);
        Task<T> GetByIdAsync(Guid id);
        Task<T> DeleteAsync(Guid id);
        Task<T> UpdateAsync(T Entity);
    }
}
