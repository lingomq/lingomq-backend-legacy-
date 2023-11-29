using Notifications.BusinessLayer.Contracts;
using Notifications.DomainLayer.Entities;

namespace Notifications.Api.Services
{
    public class DatabaseDataMigrator : IDatabaseDataMigrator
    {
        private readonly IUnitOfWork _unitOfWork;
        public DatabaseDataMigrator(IUnitOfWork unitOfWork) =>
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

        public async Task Migrate()
        {
            await AddNotificationTypes();
        }
    }
}
