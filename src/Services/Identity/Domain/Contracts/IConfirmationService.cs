using Identity.Domain.Models;

namespace Domain.Contracts;
public interface IConfirmationService
{
    Task<TokenModel> ConfirmEmailAsync(string confirmationToken, CancellationToken cancellationToken = default);
}
