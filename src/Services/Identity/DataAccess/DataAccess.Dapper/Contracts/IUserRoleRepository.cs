using Identity.Domain.Entities;

namespace DataAccess.Dapper.Contracts;
public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<UserRole?> GetByNameAsync(string name);
}
