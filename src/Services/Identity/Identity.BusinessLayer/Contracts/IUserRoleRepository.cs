using Authentication.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole, UserRole>
    {
        Task<UserRole?> GetByName(string name);
    }
}
