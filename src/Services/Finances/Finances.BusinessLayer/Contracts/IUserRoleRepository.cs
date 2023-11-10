using Finances.DomainLayer.Entities;

namespace Finances.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
