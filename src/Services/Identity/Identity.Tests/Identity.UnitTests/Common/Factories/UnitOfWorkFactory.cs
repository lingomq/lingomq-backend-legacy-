using AutoMapper;
using Identity.BusinessLayer.Contracts;
using Identity.BusinessLayer.Services;
using Identity.BusinessLayer.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Identity.UnitTests.Common.Factories
{
    public class UnitOfWorkFactory
    {
        public static UnitOfWork Create(ServiceProvider provider)
        {
            IDbConnection connection = provider.GetRequiredService<IDbConnection>();

            MapperConfiguration config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AppMappingProfile()));

            IMapper mapper = config.CreateMapper();

            ILinkTypeRepository linkTypeRepository = new LinkTypeRepository(connection);
            IUserRepository userRepository = new UserRepository(connection, mapper);
            IUserRoleRepository userRoleRepository = new UserRoleRepository(connection);
            IUserInfoRepository userInfoRepository = new UserInfoRepository(connection);
            IUserLinkRepository userLinkRepository = new UserLinkRepository(connection);
            IUserStatisticsRepository statisticsRepository = new UserStatisticsRepository(connection);

            return new UnitOfWork(linkTypeRepository, userInfoRepository, userLinkRepository,
                userRepository, userRoleRepository, statisticsRepository);
        }
    }
}
