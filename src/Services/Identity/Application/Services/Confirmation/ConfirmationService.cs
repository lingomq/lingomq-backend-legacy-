using Identity.Application.EventBus.MassTransit;
using Identity.Application.Services.Jwt;
using Identity.DataAccess.Contracts;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Models;
using LingoMqCryptographyLib;
using LingoMqCryptographyLib.Cryptors;
using LingoMqCryptographyLib.Models;
using System.Security.Claims;

namespace Identity.Application.Services.Confirmation;
public class ConfirmationService : IConfirmationService
{
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmationService(IJwtService jwtService, IUnitOfWork unitOfWork)
    {
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenModel> ConfirmEmailAsync(string confirmationToken, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(confirmationToken);

        User user = await InitializeUserAsync(principal);

        UserSensitiveData sensitiveData = new UserSensitiveData();
        InitializeUserSensitiveDatas(ref sensitiveData,
            principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)!.Value);
        sensitiveData.UserId = user.Id;
        sensitiveData.User = user;

        UserInfo userInfo = new UserInfo();
        userInfo.UserId = user.Id;
        userInfo.User = user;

        UserCredentials credentials = new UserCredentials();
        credentials.Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
        credentials.UserId = user.Id;
        credentials.User = user;

        await CreateUserAsync(user, userInfo, credentials, sensitiveData, cancellationToken);

        TokenModel tokenModel = _jwtService.CreateTokenPair(user, credentials.Email);
        return tokenModel;
    }


    private async Task CreateUserAsync(User user, UserInfo info, UserCredentials credentials, UserSensitiveData sensitiveData, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.UserInfos.AddAsync(info);
        await _unitOfWork.UserSensitiveDatas.AddAsync(sensitiveData);
        await _unitOfWork.UserCredentials.AddAsync(credentials);
    }

    private void InitializeUserSensitiveDatas(ref UserSensitiveData sensitiveData, string key)
    {
        Cryptor cryptor = new Cryptor(new Sha256Alghoritm());
        BaseKeyPair keyPair = cryptor
        .Crypt(key);

        sensitiveData.PasswordHash = keyPair.Hash;
        sensitiveData.PasswordSalt = keyPair.Salt;
    }

    private async Task<User> InitializeUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        User user = new User();
        user.Nickname = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        List<Role> roles = await _unitOfWork.Roles.GetAsync(0, int.MaxValue);
        Role? userRole = roles.FirstOrDefault(x => x.Name == "user");

        if (userRole is null)
            throw new NotImplementedException("The roles wasn't added");

        user.Role = userRole;
        user.RoleId = userRole!.Id;

        return user;
    }
}
