using Achievements.DomainLayer.Entities;

namespace Achievements.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
