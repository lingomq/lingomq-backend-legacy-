using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Contracts;

public interface IGenericRepository<T> where T: BaseEntity
{
    Task<T?> GetAsync(Guid id);
    Task<List<T>> GetAsync(int range);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task CreateAsync(T entity);
}