using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IGenericRepository<T> where T : EntityBase
{
    Task<List<T>> GetAsync(int range);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
