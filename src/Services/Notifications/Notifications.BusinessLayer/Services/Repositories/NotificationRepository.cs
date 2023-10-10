using System.Data;
using Dapper;
using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Services.Repositories;

public class NotificationRepository :  GenericRepository<Notification>, INotificationRepository
{
    private static readonly string Get = "SELECT notifications.id," +
                                         "title as \"Title\", " +
                                         "content as \"Content\", " +
                                         "notification_types.id, " +
                                         "notification_types.name as \"Name\" " +
                                         "FROM notifications " +
                                         "JOIN notification_types ON notification_types.id = notifications.notification_id ";
    private static readonly string GetById = Get + "WHERE notifications.id = @Id";
    private static readonly string GetRange = Get + "LIMIT @Count";
    private static readonly string Create = "INSERT INTO notifications " +
                                            "(id, title, content, notification_id) " +
                                            "VALUES (@Id, @Title, @Content, @NotificationId)";
    private static readonly string Update = "UPDATE notifications SET " +
                                            "title = @Title, " +
                                            "content = @Content, " +
                                            "notification_id = @NotificationId";
    private static readonly string Delete = "DELETE FROM notifications " +
                                            "WHERE id = @Id";

    private readonly IDbConnection _connection;
    public NotificationRepository(IDbConnection connection) : base(connection) => _connection = connection;

    public async Task<Notification?> GetAsync(Guid id)
    {
        List<Notification> notifications = await QueryAsync(GetById, new { Id = id }); 
        return notifications.FirstOrDefault();
    }

    public async Task<List<Notification>> GetAsync(int range) => await QueryAsync(GetRange, new { Count = range });

    public async Task UpdateAsync(Notification entity) => await ExecuteAsync(Update, entity);

    public async Task DeleteAsync(Guid id) => await ExecuteAsync(Delete, new { Id = id });

    public async Task CreateAsync(Notification entity) => await ExecuteAsync(Create, entity);
    
    protected override async Task<List<Notification>> QueryAsync<TE>(string sql, TE entity)
    {
        IEnumerable<Notification> values = await _connection.QueryAsync<Notification, NotificationType, Notification>(
            sql,
            (notification, notificationType) =>
            {
                notification.NotificationType = notificationType;
                return notification;
            }
            , entity, splitOn: "id");

        return values.ToList();
    }
}