using Identity.Domain.Entities;

namespace Identity.DataAccess.Dapper.Contracts;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task UpdateCredentialsAsync(User entity);
}
