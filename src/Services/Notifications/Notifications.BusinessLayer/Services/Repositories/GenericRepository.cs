using System.Data;
using System.Transactions;
using Dapper;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Services.Repositories;

public class GenericRepository<T> where T : BaseEntity
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

    protected virtual async Task<List<T>> QueryAsync<TE>(string sql, TE entity)
    {
        IEnumerable<T> values;
        values = await _connection.QueryAsync<T>(sql, entity);

        return values.ToList();
    }

    protected virtual async Task<T?> QueryFirstAsync<TE>(string sql, TE entity) =>
        await _connection.QueryFirstAsync<T>(sql, entity);

    protected virtual async Task<List<T>> QueryAsync<TE>(string sql, TE entity, Func<T, User, Notification, T> func)
    {
        IEnumerable<T> values;
        values = await _connection.QueryAsync(sql, func, entity, splitOn: "id");

        return values.ToList();
    }
}