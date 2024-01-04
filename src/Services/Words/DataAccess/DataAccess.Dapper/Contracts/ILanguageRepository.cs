using Words.Domain.Entities;

namespace DataAccess.Dapper.Contracts;
public interface ILanguageRepository : IGenericRepository<Language>
{
    Task<Language?> GetByNameAsync(string name);
}
