using Notifications.DomainLayer.Entities;

namespace Notifications.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
