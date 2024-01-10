namespace Identity.DataAccess.Contracts;
public interface IUnitOfWork
{
    IRoleRepository Roles { get; }
    IUserCredentialsRepository UserCredentials { get; }
    IUserInfoRepository UserInfos { get; }
    IUserRepository Users { get; }
    IUserSensitiveDataRepository UserSensitiveDatas { get; }
}
