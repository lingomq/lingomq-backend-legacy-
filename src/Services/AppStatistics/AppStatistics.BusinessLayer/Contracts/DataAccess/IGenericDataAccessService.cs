namespace AppStatistics.BusinessLayer.Contracts.DataAccess
{
    public interface IGenericDataAccessService<T>
    {
        Task<T?> GetAsync(string id);
        Task<List<T>> GetAsync();
        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
