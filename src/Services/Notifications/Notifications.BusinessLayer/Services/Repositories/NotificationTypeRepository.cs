using System.Data;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Services.Repositories;

public class NotificationTypeRepository : GenericRepository<NotificationType>, INotificationTypeRepository
{
    private static readonly string Get = "SELECT id as \"Id\", " +
                                         "name as \"Name\" " +
                                         "FROM notification_types ";

    private static readonly string GetById = Get + "WHERE id = @Id";
    private static readonly string GetRange = Get + "LIMIT @Count";
    private static readonly string Create = "INSERT INTO notification_types " +
                                            "(id, name) " +
                                            "VALUES (@Id, @Name)";

    private static readonly string Update = "UPDATE notification_types SET" +
                                            "name = @Name WHERE id = @Id";
    private static readonly string Delete = "DELETE FROM notification_types WHERE id = @Id";
    
    public NotificationTypeRepository(IDbConnection connection) : base(connection) {}

    public async Task<NotificationType?> GetAsync(Guid id) => await QueryFirstAsync(GetById, new { Id = id });

    public async Task<List<NotificationType>> GetAsync(int range) => await QueryAsync(GetRange, new { Count = range });

    public async Task UpdateAsync(NotificationType entity) => await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) => await ExecuteAsync(Delete, new { Id = id });

    public async Task CreateAsync(NotificationType entity) => await ExecuteAsync(Create, entity);
}