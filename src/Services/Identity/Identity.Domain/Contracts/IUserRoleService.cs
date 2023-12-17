using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserRoleService
{
    Task<UserRole?> GetById(Guid id);
    Task<List<UserRole>> GetRangeAsync(int count);
    Task CreateAsync(UserRole userRole, CancellationToken cancellationToken);
    Task UpdateAsync(UserRole userRole, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id);
}
