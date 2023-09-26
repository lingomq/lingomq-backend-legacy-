using Authentication.BusinessLayer.Contracts;
using Authentication.BusinessLayer.Services.Repositories;
using Authentication.BusinessLayer.Services;
using AutoMapper;
using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.UnitTest.Common.Factories
{
    public class UnitOfWorkFactory
    {
        public static UnitOfWork Create(ServiceProvider provider)
        {
            IDbConnection connection = provider.GetRequiredService<IDbConnection>();

            MapperConfiguration config = new MapperConfiguration(cfg =>
                cfg.AddProfile(new AppMappingProfile()));

            IMapper mapper = config.CreateMapper();

            IUserRepository userRepository = new UserRepository(connection);
            IUserRoleRepository userRoleRepository = new UserRoleRepository(connection);
            IUserInfoRepository userInfoRepository = new UserInfoRepository(connection, mapper);

            return new UnitOfWork(userRepository, userInfoRepository,
                userRoleRepository);
        }
    }
}
