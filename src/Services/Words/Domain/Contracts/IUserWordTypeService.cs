using Words.Domain.Entities;

namespace Words.Domain.Contracts;
public interface IUserWordTypeService
{
    Task<List<UserWordType>> GetRangeAsync(int range);
    Task<List<UserWordType>> GetByWordIdAsync(Guid id);
    Task<List<UserWordType>> GetByTypeIdAsync(Guid id);
    Task CreateAsync(UserWordType userWordType);
    Task DeleteAsync(Guid id);
}
