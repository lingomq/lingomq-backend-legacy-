using Dapper;
using Notifications.DataAccess.Dapper.Contracts;
using Notifications.DataAccess.Dapper.Postgres.RawQueries;
using Notifications.DataAccess.Dapper.Utils;
using Notifications.Domain.Entities;
using System.Data;

namespace Notifications.DataAccess.Dapper.Postgres.Realizations;
public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    private readonly IDbConnection _connection;
    public NotificationRepository(IDbConnection connection) : base(connection) =>
        _connection = connection;

    public async Task<Notification?> GetAsync(Guid id) =>
        await QueryFirstAsync(NotificationQueries.GetById, new { Id = id });

    public async Task<List<Notification>> GetAsync(int count) =>
        await QueryListAsync(NotificationQueries.GetRange, new { Count = count });

    public async Task CreateAsync(Notification entity) =>
        await ExecuteByTemplateAsync(NotificationQueries.Create, entity);

    public async Task UpdateAsync(Notification entity) =>
        await ExecuteByTemplateAsync(NotificationQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(NotificationQueries.Delete, new { Id = id });

    protected override async Task<Notification?> QueryFirstAsync<E>(string sql, E entity)
    {
        List<Notification> notifications = await QueryListAsync(sql, entity);
        return notifications.Any() ? notifications.FirstOrDefault() : null;
    }

    protected override async Task<List<Notification>> QueryListAsync<E>(string sql, E entity)
    {
        IEnumerable<Notification> values = await _connection.QueryAsync(
            sql,
            (Notification notification, Domain.Entities.NotificationType notificationType) =>
            {
                notification.NotificationType = notificationType;
                return notification;
            }
            , entity, splitOn: "id");

        return values.Any() ? values.ToList() : new List<Notification>();
    }
}
