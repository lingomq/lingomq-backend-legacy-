using DataAccess.Dapper.Contracts;
using DataAccess.Dapper.Postgres.RawQueries;
using DataAccess.Dapper.Utils;
using System.Data;
using Words.Domain.Entities;

namespace DataAccess.Dapper.Postgres.Realizations;
public class WordTypeRepository : GenericRepository<WordType>, IWordTypeRepository
{
    private readonly IDbConnection _connection;
    public WordTypeRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;
    public async Task AddAsync(WordType entity) =>
        await ExecuteByTemplateAsync(WordTypeQueries.Create, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(WordTypeQueries.Delete, new { Id = id });

    public async Task<List<WordType>> GetAsync(int range = int.MaxValue) =>
        await QueryListAsync(WordTypeQueries.GetRange, new { Count = range });

    public async Task<WordType?> GetByIdAsync(Guid id) =>
        await QueryFirstAsync(WordTypeQueries.GetById, new { Id = id });

    public async Task UpdateAsync(WordType entity) =>
        await ExecuteByTemplateAsync(WordTypeQueries.Update, entity);
}
