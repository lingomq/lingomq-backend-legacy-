using Identity.Domain.Entities;

namespace Identity.Domain.Contracts;
public interface IUserRoleService
{
    Task<UserRole> GetByIdAsync(Guid id);
    Task<List<UserRole>> GetRangeAsync(int count);
    Task CreateAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
