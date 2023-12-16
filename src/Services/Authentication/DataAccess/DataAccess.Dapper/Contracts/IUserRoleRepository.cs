using Authentication.Domain.Contracts;
using Authentication.Domain.Entities;

namespace Authentication.DataAccess.Dapper.Contracts;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<UserRole?> GetByNameAsync(string name);
}

