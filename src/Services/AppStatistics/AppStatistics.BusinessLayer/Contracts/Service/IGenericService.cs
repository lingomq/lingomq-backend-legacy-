namespace AppStatistics.BusinessLayer.Contracts.Service
{
    public interface IGenericService<T>
    {
        Task<T?> GetAsync(string id);
        Task<List<T>> GetAsync();
        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
