using Authentication.BusinessLayer.Contracts;

namespace Authentication.BusinessLayer.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository Users { get; }
        public IUserInfoRepository UserInfos { get; }
        public IUserRoleRepository UserRoles { get; }
        public UnitOfWork(IUserRepository users,
            IUserInfoRepository userInfos,
            IUserRoleRepository userRoles)
        {
            Users = users;
            UserInfos = userInfos;
            UserRoles = userRoles;
        }
    }
}
