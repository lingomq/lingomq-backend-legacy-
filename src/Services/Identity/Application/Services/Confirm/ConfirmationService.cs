using Identity.Application.EventBus.MassTransit;
using Identity.Domain.Exceptions.ClientExceptions;
using Identity.DataAccess.Dapper.Contracts;
using Identity.Application.Services.Jwt;
using Identity.Domain.Entities;
using Identity.Domain.Contracts;
using Identity.Domain.Models;
using System.Security.Claims;
using Cryptography;
using Cryptography.Entities;
using Cryptography.Cryptors;
using Identity.Application.Services.Confirm.Extensions;
using EventBus.Entities.Identity.User;
using EventBus.Entities.AppStatistics;

namespace Identity.Application.Services.Confirm;
public class ConfirmationService : IConfirmationService
{
    private IJwtService _jwtService;
    private IUnitOfWork _unitOfWork;
    private PublisherBase _publisher;
    public ConfirmationService(IJwtService jwtService, IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task<TokenModel> ConfirmEmailAsync(string token, CancellationToken cancellationToken = default)
    {
        ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);

        Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
        BaseKeyPair keyPair = cryptor
        .Crypt(principal.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.Authentication)!.Value);

        User user = new User();
        user.SetUserModel(principal, keyPair);

        UserRole? userRole = await _unitOfWork.UserRoles.GetByNameAsync("user");

        UserInfo userInfo = new UserInfo().SetUserInfo(user, principal, userRole!);

        if (await _unitOfWork.Users.GetByEmailAsync(user.Email!) is not null ||
        await _unitOfWork.UserInfos.GetByNicknameAsync(userInfo.Nickname!) is not null)
            throw new ConflictException<User>();

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.UserInfos.AddAsync(userInfo);

        TokenModel tokenModel = _jwtService.CreateTokenPair(userInfo);
        await _publisher.Send(new IdentityModelCreateUser().SetValues(user, userInfo, userRole!), cancellationToken);
        await _publisher.Send(new AppStatisticsCreateOrUpdate()
        {
            TotalUsers = 1,
            Date = DateTime.UtcNow
        }, cancellationToken);

        return tokenModel;
    }
}
