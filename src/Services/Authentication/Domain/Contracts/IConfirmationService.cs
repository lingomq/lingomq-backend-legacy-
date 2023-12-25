using Authentication.Domain.Models;

namespace Authentication.Domain.Contracts;

public interface IConfirmationService
{
    Task<TokenModel> ConfirmEmailAsync(string token, CancellationToken cancellationToken);
}

