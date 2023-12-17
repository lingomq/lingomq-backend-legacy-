using Words.Domain.Entities;

namespace Words.Domain.Contracts;
public interface ILanguageService
{
    Task<Language> GetAsync(int count);
    Task<Language> GetAsync(Guid id);
    Task CreateAsync(Language language);
    Task UpdateAsync(Language language);
    Task DeleteAsync(Guid id);
}
