using Notifications.DataAccess.Dapper.Contracts;
using Notifications.Domain.Entities;

namespace Notifications.Application.Services.DataMigrator;
public class DataMigrator : IDataMigrator
{
    private readonly IUnitOfWork _unitOfWork;
    public DataMigrator(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task AddNotificationTypes()
    {
        List<string> typeNames = new List<string>() { "important", "attention", "usual" };
        var typesFromDatabase = await _unitOfWork.NotificationTypes.GetAsync(10);

        foreach (var typeName in typesFromDatabase.Select(x => x.Name))
        {
            if (typeNames.Contains(typeName!))
                typeNames.Remove(typeName!);
        }

        foreach (string typeName in typeNames)
            await _unitOfWork.NotificationTypes.CreateAsync(new NotificationType() { Id = Guid.NewGuid(), Name = typeName });
    }

    public async Task MigrateAsync()
    {
        await AddNotificationTypes();
    }
}
