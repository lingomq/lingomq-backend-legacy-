using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Contracts;
public interface IGenericRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAsync(int range);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
