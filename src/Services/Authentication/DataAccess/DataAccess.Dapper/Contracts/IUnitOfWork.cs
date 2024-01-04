namespace Authentication.DataAccess.Dapper.Contracts;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IUserInfoRepository UserInfos { get; }
    IUserRoleRepository UserRoles { get; }
}

