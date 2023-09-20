namespace Authentication.BusinessLayer.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IUserInfoRepository UserInfos { get; }
        IUserRoleRepository UserRoles { get; }
    }
}
