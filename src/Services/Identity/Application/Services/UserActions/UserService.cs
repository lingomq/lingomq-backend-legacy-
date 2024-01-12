using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;

namespace Identity.Application.Services.UserActions;
public class UserService : IUserService
{
    public Task<OperationStatus> CreateAsync(Domain.Entities.User user, UserInfo info, UserCredentials credentials, UserSensitiveData sensitiveData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserInfo> GetInfoAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Entities.User> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Domain.Entities.User>> GetUsersAsync(int take, int skip = 20, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> UpdateCredentialsAsync(UserCredentials credentials, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> UpdateInfoAsync(UserInfo info, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> UpdateSensitiveDataAsync(UserSensitiveData sensitiveData, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> UpdateUserAsync(Domain.Entities.User user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
