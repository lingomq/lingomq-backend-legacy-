using Topics.DomainLayer.Entities;

namespace Topics.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
