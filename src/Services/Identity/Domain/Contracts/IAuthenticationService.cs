using Identity.Domain.Constants;
using Identity.Domain.Entities;
using Identity.Domain.Models;

namespace Domain.Contracts;
public interface IAuthenticationService
{
    Task<bool> ValidateSensitiveDataAsync(ref UserSensitiveData data, CancellationToken cancellationToken = default);
    Task<TokenModel> SignInAsync(AuthenticationModel model, CancellationToken cancellationToken = default);
    Task<OperationStatus> SignUpAsync(AuthenticationModel model, CancellationToken cancellationToken = default);
    Task<TokenModel> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
