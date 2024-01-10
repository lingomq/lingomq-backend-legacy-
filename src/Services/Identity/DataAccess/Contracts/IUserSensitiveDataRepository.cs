using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserSensitiveDataRepository
{
    Task<UserSensitiveData?> GetByUserIdAsync(Guid id);
    Task<UserSensitiveData?> GetByUserNicknameAsync(string nickname);
    Task AddAsync(UserSensitiveData entity);
    Task UpdateAsync(UserSensitiveData entity);
    Task DeleteAsync(Guid id);
}
