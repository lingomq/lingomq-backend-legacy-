using Identity.Domain.Constants;
using Identity.Domain.Contracts;
using Identity.Domain.Entities;
using Identity.Domain.Models;

namespace Identity.Application.Services.Authentication;
public class AuthenticationService : IAuthenticationService
{
    public Task<TokenModel> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TokenModel> SignInAsync(AuthenticationModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<OperationStatus> SignUpAsync(AuthenticationModel model, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateSensitiveDataAsync(ref UserSensitiveData data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
