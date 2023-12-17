using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserInfoService
{
    Task<UserInfo?> GetByIdAsync(Guid id);
    Task<List<UserInfo>> GetRangeAsync(int count);
    Task UpdateAsync(UserInfo? userInfo, CancellationToken cancellationToken);
}
