using Identity.DataAccess.Contracts;
using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;

namespace Identity.Application.Services.UserActions;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationStatus> CreateAsync(User user, UserInfo info, UserCredentials credentials, UserSensitiveData sensitiveData, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await CheckIsExistanceUserAsync(user, cancellationToken))
            return OperationStatus.Conflict;
        if (await CheckIsExistanceCredentialsAsync(credentials, cancellationToken))
            return OperationStatus.Conflict;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.UserSensitiveDatas.AddAsync(sensitiveData);
        await _unitOfWork.UserCredentials.AddAsync(credentials);
        await _unitOfWork.UserInfos.AddAsync(info);

        return OperationStatus.Success;
    }

    public async Task<OperationStatus> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        User? user = await _unitOfWork.Users.GetAsync(userId);
        if (user is null)
            return OperationStatus.Error;

        await _unitOfWork.Users.DeleteAsync(userId);

        return OperationStatus.Success;
    }

    public async Task<UserInfo> GetInfoAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        UserInfo? info = await _unitOfWork.UserInfos.GetByUserIdAsync(userId);
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        return info;
    }

    public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        User? user = await _unitOfWork.Users.GetAsync(id);
        if (user is null)
            throw new ArgumentException(nameof(user));

        return user;
    }

    public async Task<List<User>> GetUsersAsync(int take, int skip = 20, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<User> users = await _unitOfWork.Users.GetAsync(take, skip);
        return users;
    }

    public async Task<OperationStatus> UpdateCredentialsAsync(UserCredentials credentials, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        bool isExistForConcreteUser = await _unitOfWork.UserCredentials.GetByUserIdAsync(credentials.UserId) is null;

        if (!isExistForConcreteUser)
            return OperationStatus.Error;
        if (await CheckIsExistanceCredentialsAsync(credentials, cancellationToken) && !isExistForConcreteUser)
            return OperationStatus.Conflict;

        await _unitOfWork.UserCredentials.UpdateAsync(credentials);

        return OperationStatus.Success;
    }

    public async Task<OperationStatus> UpdateInfoAsync(UserInfo info, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _unitOfWork.UserInfos.GetByUserIdAsync(info.UserId) is null)
            return OperationStatus.Error;

        await _unitOfWork.UserInfos.UpdateAsync(info);

        return OperationStatus.Success;
    }

    public async Task<OperationStatus> UpdateSensitiveDataAsync(UserSensitiveData sensitiveData, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();

        if (await _unitOfWork.UserSensitiveDatas.GetByUserIdAsync(sensitiveData.UserId) is null)
            return OperationStatus.Error;

        await _unitOfWork.UserSensitiveDatas.UpdateAsync(sensitiveData);
        return OperationStatus.Success;
    }

    public async Task<OperationStatus> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _unitOfWork.Users.GetAsync(user.Id) is null)
            return OperationStatus.Error;

        await _unitOfWork.Users.UpdateAsync(user);
        return OperationStatus.Success;
    }

    private async Task<bool> CheckIsExistanceUserAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        List<User> users = await _unitOfWork.Users.GetAsync(0, int.MaxValue);

        if (users.FirstOrDefault(x => x.Nickname == user.Nickname) is not null)
            return true;
        if (users.FirstOrDefault(x => x.Id == user.Id) is not null)
            return true;

        return false;
    }

    private async Task<bool> CheckIsExistanceCredentialsAsync(UserCredentials credentials, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<UserCredentials> userCredentials = await _unitOfWork.UserCredentials.GetAsync(0, int.MaxValue);

        if (userCredentials.FirstOrDefault(x => x.Email == credentials.Email) is not null)  
            return true;
        if (userCredentials.FirstOrDefault(x => x.Phone == credentials.Phone) is not null)
            return true;

        return false;
    }
}
