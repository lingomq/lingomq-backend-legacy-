using Identity.DataAccess.Contracts;

namespace Identity.DataAccess.Providers.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public IRoleRepository Roles { get; }
    public IUserCredentialsRepository UserCredentials { get; }
    public IUserInfoRepository UserInfos { get; }
    public IUserRepository Users { get; }
    public IUserSensitiveDataRepository UserSensitiveDatas { get; }

    public UnitOfWork(IRoleRepository roles,
        IUserCredentialsRepository userCredentials,
        IUserInfoRepository userInfos,
        IUserRepository users,
        IUserSensitiveDataRepository userSensitiveDatas)
    {
        Roles = roles;
        UserCredentials = userCredentials;
        UserInfos = userInfos;
        Users = users;
        UserSensitiveDatas = userSensitiveDatas;
    }
}
