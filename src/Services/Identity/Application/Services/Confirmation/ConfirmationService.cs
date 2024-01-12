using Identity.Domain.Contracts;
using Identity.Domain.Models;

namespace Identity.Application.Services.Confirmation;
public class ConfirmationService : IConfirmationService
{
    public Task<TokenModel> ConfirmEmailAsync(string confirmationToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
