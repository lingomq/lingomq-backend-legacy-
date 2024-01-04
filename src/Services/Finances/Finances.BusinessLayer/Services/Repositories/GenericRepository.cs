using System.Data;
using System.Transactions;
using Dapper;

namespace Finances.BusinessLayer.Services.Repositories;

public abstract class GenericRepository<T> where T: class
{
    private readonly IDbConnection _connection;

    protected GenericRepository(IDbConnection connection) =>
        _connection = connection;

    protected virtual async Task ExecuteAsync<TE>(string sql, TE entity)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _connection.ExecuteAsync(sql, entity);
        transactionScope.Complete();
        transactionScope.Dispose();
    }

    protected virtual async Task<T> QuerySingleAsync<TE>(string sql, TE entity) =>
        await _connection.QueryFirstAsync<T>(sql, entity);

    protected virtual async Task<List<T>> QueryAsync<TE>(string sql, TE entity)
    {
        IEnumerable<T> values = await _connection.QueryAsync<T>(sql, entity);
        return values.ToList();
    }
}