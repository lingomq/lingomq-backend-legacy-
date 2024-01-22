using DataAccess.Dapper.Contracts;
using Identity.Domain.Entities;

namespace Identity.Application.Services.DataMigrator;
public class DataMigrator : IDataMigrator
{
    private readonly IUnitOfWork _unitOfWork;
    public DataMigrator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task AddRolesAsync()
    {
        List<string> roleNames = new List<string>() { "user", "admin", "moderator" };
        var rolesFromDatabase = await _unitOfWork.UserRoles.GetAsync(int.MaxValue);

        foreach (var role in rolesFromDatabase)
        {
            if (roleNames.Contains(role.Name!))
                roleNames.Remove(role.Name!);
        }

        foreach (string roleName in roleNames)
            await _unitOfWork.UserRoles.AddAsync(new UserRole() { Id = Guid.NewGuid(), Name = roleName });
    }

    public async Task MigrateAsync()
    {
        await AddRolesAsync();
    }
}
