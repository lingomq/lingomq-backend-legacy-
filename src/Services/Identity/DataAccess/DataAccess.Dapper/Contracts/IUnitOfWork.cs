namespace Identity.DataAccess.Dapper.Contracts;
public interface IUnitOfWork
{
    public IUserInfoRepository UserInfos { get; }
    public IUserRepository Users { get; }
    public IUserRoleRepository UserRoles { get; }
    public IUserStatisticsRepository UserStatistics { get; }
}
