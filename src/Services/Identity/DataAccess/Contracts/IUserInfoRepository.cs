using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserInfoRepository : IGenericRepository<UserInfo>
{
    Task<UserInfo?> GetByUserIdAsync(Guid id);
}
