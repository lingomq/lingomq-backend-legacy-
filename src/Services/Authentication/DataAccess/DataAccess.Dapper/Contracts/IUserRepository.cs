using Authentication.Domain.Contracts;
using Authentication.Domain.Entities;

namespace Authentication.DataAccess.Dapper.Contracts;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> UpdateCredentialsAsync(User user);
}

