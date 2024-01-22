using Identity.Domain.Entities;

namespace Identity.DataAccess.Dapper.Contracts;

public interface IUserInfoRepository : IGenericRepository<UserInfo>
{
    Task<UserInfo?> GetByNicknameAsync(string nickname);
    Task<UserInfo?> GetByUserIdAsync(Guid id);
}
