using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task UpdateCredentialsAsync(User user, UserCredentialsModel userCredentialsModel, CancellationToken cancellationToken);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}

public class UserCredentialsModel
{
    public string? Password { get; set; }
}