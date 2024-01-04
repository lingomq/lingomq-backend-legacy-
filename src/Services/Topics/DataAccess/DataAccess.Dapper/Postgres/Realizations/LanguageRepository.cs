using Topics.DataAccess.Dapper.Contracts;
using Topics.Domain.Entities;
using Topics.DataAccess.Dapper.Utils;
using System.Data;
using Topics.DataAccess.Dapper.Postgres.RawQueries;

namespace Topics.DataAccess.Dapper.Postgres.Realizations;
public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
{
    public LanguageRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task AddAsync(Language entity) =>
        await ExecuteByTemplateAsync(LanguageQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(LanguageQueries.Delete, new { Id = id });

    public async Task<List<Language>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(LanguageQueries.GetRange, new { Count = range });

    public async Task<Language?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(LanguageQueries.GetById, new { Id = id });

    public async Task UpdateAsync(Language entity) =>
        await ExecuteByTemplateAsync(LanguageQueries.Update, entity);
}
