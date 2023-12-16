using Authentication.DataAccess.Dapper.Contracts;

namespace Authentication.DataAccess.Dapper;
public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IUserRepository userRepository, IUserInfoRepository userInfoRepository, IUserRoleRepository userRoleRepository) 
    {
        Users = userRepository;
        UserInfos = userInfoRepository;
        UserRoles = userRoleRepository;
    }
    public IUserRepository Users { get; }

    public IUserInfoRepository UserInfos { get; }

    public IUserRoleRepository UserRoles { get; }
}
