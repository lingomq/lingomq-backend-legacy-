using Authentication.Application.EventBus.MassTransit;
using Authentication.Application.Services.Jwt;
using Authentication.DataAccess.Dapper.Contracts;
using Authentication.Domain.Contracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Models;
using Domain.Validations;
using EventBus.Entities.Email;
using System.Security.Claims;

namespace Authentication.Application.Services.Authentication;
public class AuthenticationService : IAuthenticationService
{
    private IJwtService _jwtService;
    private IUnitOfWork _unitOfWork;
    private PublisherBase _publisher;
    public AuthenticationService(IJwtService jwtService, IUnitOfWork unitOfWork, PublisherBase publisher)
    {
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task<TokenModel> RefreshTokenAsync(string token, CancellationToken cancellationToken)
    {
        ClaimsPrincipal principal = _jwtService.GetClaimsPrincipal(token);
        if (principal is null)
            throw new InvalidTokenException<User>();

        UserInfo? info = await _unitOfWork.UserInfos
            .GetByNicknameAsync(
            principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value);

        return _jwtService.CreateTokenPair(info!);

    }

    public async Task<int> SignUpAsync(SignModel signUpModel, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.GetByEmailAsync(signUpModel.Email!) is not null ||
                await _unitOfWork.UserInfos.GetByNicknameAsync(signUpModel.Nickname!) is not null)
            throw new ConflictException<User>();

        string emailToken = _jwtService.GenerateEmailToken(signUpModel);

        await _publisher.Send(new EmailModelSignUp()
        {
            Email = signUpModel.Email!,
            Nickname = signUpModel.Nickname!,
            Token = emailToken,
            Subject = "Подтверждение аккаунта"
        }, cancellationToken);

        return 0;
    }

    public async Task<TokenModel> SignInAsync(SignModel signUpModel, CancellationToken cancellationToken = default)
    {
        UserInfo? info = await _unitOfWork.UserInfos.GetByNicknameAsync(signUpModel.Nickname!);
        User? user = await _unitOfWork.Users.GetByEmailAsync(signUpModel.Email!);

        ValidateSignIn(info, user, signUpModel);

        TokenModel tokenModel = _jwtService.CreateTokenPair(info!);

        await _publisher.Send(new EmailModelSignIn()
        {
            Email = signUpModel.Email!,
            Nickname = signUpModel.Nickname!,
            Subject = "Вход в аккаунт"
        }, cancellationToken);

        return tokenModel;
    }

    private void ValidateSignIn(UserInfo? info, User? user, SignModel signModel)
    {
        if (info is null || user is null)
            throw new NotFoundException<User>();

        if (!info.UserId.Equals(user.Id))
            throw new NotFoundException<User>();

        if (!CredentialsValidator.IsValidPassword(ref user, signModel.Password!))
            throw new InvalidDataException<User>();
    }
}
