using Identity.DataAccess.Dapper.Contracts;

namespace Identity.DataAccess.Dapper;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IUserInfoRepository UserInfos { get; }

    public IUserRepository Users { get; }

    public IUserRoleRepository UserRoles { get; }

    public IUserStatisticsRepository UserStatistics { get; }

    public UnitOfWork(IUserInfoRepository userInfoRepository,
        IUserRepository userRepository,
        IUserRoleRepository roleRepository,
        IUserStatisticsRepository userStatisticsRepository)
    {
        UserInfos = userInfoRepository;
        Users = userRepository;
        UserRoles = roleRepository;
        UserStatistics = userStatisticsRepository;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
