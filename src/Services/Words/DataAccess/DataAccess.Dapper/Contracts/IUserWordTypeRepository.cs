using Words.Domain.Entities;

namespace DataAccess.Dapper.Contracts;
public interface IUserWordTypeRepository : IGenericRepository<UserWordType>
{
    Task<List<UserWordType>> GetByUserIdAsync(Guid id);
    Task<List<UserWordType>> GetByTypeIdAsync(Guid id);
}
