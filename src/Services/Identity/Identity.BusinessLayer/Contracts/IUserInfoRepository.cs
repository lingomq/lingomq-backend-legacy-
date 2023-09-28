using Authentication.DomainLayer.Entities;

namespace Identity.BusinessLayer.Contracts
{
    public interface IUserInfoRepository : IGenericRepository<UserInfo, UserInfo>
    {
        Task<UserInfo?> GetByNicknameAsync(string nickname);
    }
}
