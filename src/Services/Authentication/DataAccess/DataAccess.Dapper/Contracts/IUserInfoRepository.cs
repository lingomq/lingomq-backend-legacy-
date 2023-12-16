using Authentication.Domain.Contracts;
using Authentication.Domain.Entities;

namespace Authentication.DataAccess.Dapper.Contracts;

public interface IUserInfoRepository : IGenericRepository<UserInfo>
{
    Task<UserInfo?> GetByNicknameAsync(string nickname);
}

