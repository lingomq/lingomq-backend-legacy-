using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid id);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task UpdateCredentialsAsync(UserCredentialsModel userCredentialsModel, CancellationToken cancellationToken);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}

public class UserCredentialsModel
{
    public Guid Id { get; set; }
    public string Password { get; set; } = "";
}