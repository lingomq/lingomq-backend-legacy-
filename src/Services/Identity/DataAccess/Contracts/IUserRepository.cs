using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetAsync(Guid id);
}
