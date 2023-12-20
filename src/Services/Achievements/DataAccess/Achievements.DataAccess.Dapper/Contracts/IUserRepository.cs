using Achievements.Domain.Entities;

namespace Achievements.DataAccess.Dapper.Contracts;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
