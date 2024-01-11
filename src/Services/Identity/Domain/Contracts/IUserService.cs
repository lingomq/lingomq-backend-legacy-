using Identity.Domain.Constants;
using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserService
{
    Task<List<User>> GetUsersAsync(int take, int skip = 20, CancellationToken cancellationToken = default);
    Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserInfo> GetInfoAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<OperationStatus> CreateAsync(User user, UserInfo info, UserCredentials credentials, UserSensitiveData sensitiveData, CancellationToken cancellationToken = default);
    Task<OperationStatus> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<OperationStatus> UpdateSensitiveDataAsync(UserSensitiveData sensitiveData, CancellationToken cancellation = default);
    Task<OperationStatus> UpdateCredentialsAsync(UserCredentials credentials, CancellationToken cancellationToken = default);
    Task<OperationStatus> UpdateInfoAsync(UserInfo info, CancellationToken cancellationToken = default);
    Task<OperationStatus> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
