using System.Data;
using Notifications.DataAccess.Dapper.Contracts;
using Notifications.DataAccess.Dapper.Postgres.RawQueries;
using Notifications.DataAccess.Dapper.Utils;
using Notifications.Domain.Entities;

namespace Notifications.DataAccess.Dapper.Postgres.Realizations;
public class NotificationTypeRepository : GenericRepository<NotificationType>, INotificationTypeRepository
{
    public NotificationTypeRepository(IDbConnection connection) : base(connection)
    {
    }

    public async Task<NotificationType?> GetAsync(Guid id) =>
        await QueryFirstAsync(NotificationTypeQueries.GetById, new { Id = id });

    public async Task<List<NotificationType>> GetAsync(int count) =>
        await QueryListAsync(NotificationTypeQueries.GetRange, new { Count = count });

    public async Task CreateAsync(NotificationType entity) =>
        await ExecuteByTemplateAsync(NotificationTypeQueries.Create, entity);

    public async Task UpdateAsync(NotificationType entity) =>
        await ExecuteByTemplateAsync(NotificationTypeQueries.Update, entity);

    public async Task DeleteAsync(Guid id) =>
        await ExecuteByTemplateAsync(NotificationTypeQueries.Delete, new { Id = id });
}
