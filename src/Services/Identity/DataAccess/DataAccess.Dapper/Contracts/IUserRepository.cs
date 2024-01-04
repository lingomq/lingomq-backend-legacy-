using Domain.Dtos;
using Identity.Domain.Entities;

namespace DataAccess.Dapper.Contracts;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task UpdateCredentialsAsync(User entity);
}
