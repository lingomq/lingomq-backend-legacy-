using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserCredentials : IGenericRepository<UserCredentials>
{
    Task<UserInfo?> GetByUserIdAsync(Guid id);
}
