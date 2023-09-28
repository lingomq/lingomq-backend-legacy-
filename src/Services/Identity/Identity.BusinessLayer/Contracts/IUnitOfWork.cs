namespace Identity.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        public ILinkTypeRepository LinkTypes { get; }
        public IUserInfoRepository UserInfos { get; }
        public IUserLinkRepository UserLinks { get; }
        public IUserRepository Users { get; }
        public IUserRoleRepository UserRoles { get; }
        public IUserStatisticsRepository UserStatistics { get; }
    }
}
