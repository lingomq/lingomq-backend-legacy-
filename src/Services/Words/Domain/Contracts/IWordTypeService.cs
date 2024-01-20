using Words.Domain.Entities;

namespace Words.Domain.Contracts;
public interface IWordTypeService
{
    Task<List<WordType>> GetRangeAsync(int range);
    Task<WordType> GetAsync(Guid id);
    Task CreateAsync(WordType wordType);
    Task UpdateAsync(WordType wordType);
    Task DeleteAsync(Guid id);
}
