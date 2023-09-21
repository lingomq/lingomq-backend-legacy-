using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IGenericRepository<T, E> where T : BaseEntity where E : BaseEntity
    {
        Task<List<T>?> GetAsync(int count = 0);
        Task<T?> GetByGuidAsync(Guid guid);
        Task<T> AddAsync(E entity);
        Task<T> UpdateAsync(E entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
