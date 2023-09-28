namespace Identity.BusinessLayer.Contracts
{
    public interface IGenericRepository<T, D> where T : class where D : class
    {
        Task<List<D>> GetAsync(int count);
        Task<D?> GetByIdAsync(Guid id);
        Task<D> DeleteAsync(Guid id);
        Task<D> AddAsync(T entity);
        Task<D> UpdateAsync(T entity);
    }
}
