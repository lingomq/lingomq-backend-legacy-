using Identity.Domain.Models;

namespace Identity.Domain.Contracts;
public interface IConfirmationService
{
    Task<TokenModel> ConfirmEmailAsync(string token, CancellationToken cancellationToken);
}
