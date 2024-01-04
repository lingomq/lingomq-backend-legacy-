using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Contracts;
public interface IGenericRepository<T> where T : EntityBase
{
    Task AddAsync(T entity);
    Task<List<T>> GetAsync(int range = int.MaxValue);
    Task<T?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(T entity);
}
