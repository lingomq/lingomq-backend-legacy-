using Identity.Domain.Entities;

namespace Identity.DataAccess.Contracts;
public interface IUserSensitiveDataRepository : IGenericRepository<UserSensitiveData>
{
    Task<UserInfo?> GetByUserNicknameAsync(Guid id);
}
