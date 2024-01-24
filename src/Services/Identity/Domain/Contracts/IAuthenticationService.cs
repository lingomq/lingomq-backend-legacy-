using Identity.Domain.Models;

namespace Identity.Domain.Contracts;
public interface IAuthenticationService
{
    Task<TokenModel> SignInAsync(SignModel signUpModel, CancellationToken cancellationToken);
    Task<int> SignUpAsync(SignModel signUpModel, CancellationToken cancellationToken);
    Task<TokenModel> RefreshTokenAsync(string token, CancellationToken cancellationToken);
}
