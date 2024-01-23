using Identity.DataAccess.Dapper.Contracts;
using Domain.Contracts.Extensions;
using EventBus.Entities.Identity.UserInfo;
using Identity.Application.EventBus.MassTransit;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions.ClientExceptions;

namespace Identity.Application.Services.UserInfoActions;
public class UserInfoService : IUserInfoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PublisherBase _publisher;
    public UserInfoService(IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<UserInfo> GetByIdAsync(Guid id)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException<User>("Пользователь не найден");

        UserInfo? info = await _unitOfWork.UserInfos.GetByUserIdAsync(id);
        if (info is null)
            throw new NotFoundException<UserInfo>("Данные не найдены");

        return info;
    }

    public async Task<List<UserInfo>> GetRangeAsync(int count)
    {
        return await _unitOfWork.UserInfos.GetAsync(count);
    }

    public async Task UpdateAsync(UserInfo? userInfo, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserInfos.UpdateAsync(userInfo!);
        await _publisher.Send(new IdentityModelUpdateUserInfo().ToConsume(userInfo!), cancellationToken);
    }
}
