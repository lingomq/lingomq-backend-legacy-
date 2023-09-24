using Authentication.BusinessLayer.Dtos;
using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Contracts
{
    public interface IUserInfoRepository : IGenericRepository<UserInfoDto, UserInfo> 
    {
        Task<UserInfoDto?> GetByNicknameAsync(string nickname);
    }
}
