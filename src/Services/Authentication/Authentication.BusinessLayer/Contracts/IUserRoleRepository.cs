using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole, UserRole> 
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
