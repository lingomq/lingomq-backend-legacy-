using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserCredentialsRepository : IGenericRepository<UserCredentials>
{
    Task<UserCredentials?> GetByUserIdAsync(Guid id);
}
