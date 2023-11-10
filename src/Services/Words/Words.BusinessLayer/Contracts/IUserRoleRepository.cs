using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Contracts
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<UserRole?> GetByNameAsync(string name);
    }
}
