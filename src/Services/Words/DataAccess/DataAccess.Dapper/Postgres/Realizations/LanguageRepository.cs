using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.RawQueries;
using DataAccess.Dapper.Utils;
using System.Data;
using Words.Domain.Entities;

namespace DataAccess.Dapper.Postgres.Realizations;
public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
{
    private readonly IDbConnection _connection;
    public LanguageRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task AddAsync(Language entity) =>
        await ExecuteByTemplateAsync(LanguageQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(LanguageQueries.Delete, new { Id = id });

    public async Task<List<Language>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(LanguageQueries.GetRange, new { Count = range });

    public async Task<Language?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(LanguageQueries.GetById, new { Id = id });

    public async Task<Language?> GetByNameAsync(string name) =>
        await QueryFirstAsync(LanguageQueries.GetByLanguageName, new { Name = name });

    public async Task UpdateAsync(Language entity) =>
        await ExecuteByTemplateAsync(LanguageQueries.Update, entity);
}
