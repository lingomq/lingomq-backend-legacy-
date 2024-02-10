using Finances.BusinessLayer.Contracts;
using Finances.DomainLayer.Entities;
using System.Data;

namespace Finances.BusinessLayer.Services.Repositories;

public class FinanceRepository : GenericRepository<Finance>, IFinanceRepository
{
    private static readonly string Get = "SELECT id as \"Id\"," +
                                        "name as \"FinanceName\", " +
                                        "amount as \"Amount\" " +
                                        "FROM finances ";

    private static readonly string GetRange = Get + "LIMIT @Count";

    private static readonly string GetById = Get + "WHERE id = @Id";

    private static readonly string Create = "INSERT INTO finances " +
                                            "(id, name, amount) " +
                                            "VALUES (@Id, @FinanceName, @Amount)";

    private static readonly string Update = "UPDATE finances SET " +
                                            "name = @FinanceName, " +
                                            "amount = @Amount " +
                                            "WHERE id = @Id";

    private static readonly string Delete = "DELETE from finances WHERE id = @Id";

    public FinanceRepository(IDbConnection connection) : base(connection) { }

    public async Task<Finance?> GetAsync(Guid id) =>
        await QuerySingleAsync(GetById, new { Id = id });

    public async Task<List<Finance>> GetAsync(int range) =>
        await QueryAsync(GetRange, new { Count = range });

    public async Task CreateAsync(Finance entity) =>
        await ExecuteAsync(Create, entity);

    public async Task UpdateAsync(Finance entity) =>
        await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteAsync(Delete, new { Id = id });
}