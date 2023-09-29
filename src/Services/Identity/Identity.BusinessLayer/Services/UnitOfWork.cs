using Identity.BusinessLayer.Contracts;

namespace Identity.BusinessLayer.Services
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ILinkTypeRepository LinkTypes { get; }

        public IUserInfoRepository UserInfos { get; }

        public IUserLinkRepository UserLinks { get; }

        public IUserRepository Users { get; }

        public IUserRoleRepository UserRoles { get; }

        public IUserStatisticsRepository UserStatistics { get; }

        public UnitOfWork(ILinkTypeRepository linkTypeRepository,
            IUserInfoRepository userInfoRepository,
            IUserLinkRepository userLinkRepository,
            IUserRepository userRepository,
            IUserRoleRepository roleRepository,
            IUserStatisticsRepository userStatisticsRepository) 
        {
            LinkTypes = linkTypeRepository;
            UserInfos = userInfoRepository;
            UserLinks = userLinkRepository;
            Users = userRepository;
            UserRoles = roleRepository;
            UserStatistics = userStatisticsRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
