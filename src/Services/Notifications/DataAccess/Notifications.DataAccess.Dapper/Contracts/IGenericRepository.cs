using Notifications.Domain.Entities;

namespace Notifications.DataAccess.Dapper.Contracts;
public interface IGenericRepository<T> where T : EntityBase
{
    Task<T?> GetAsync(Guid id);
    Task<List<T>> GetAsync(int count);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
