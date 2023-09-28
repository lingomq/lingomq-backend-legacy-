using Identity.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole, UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
