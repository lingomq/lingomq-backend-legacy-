using Authentication.BusinessLayer.Dtos;
using Authentication.BusinessLayer.Models;
using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Extensions
{
    public static class SignInUpResponseModelExtensions
    {
        public static UserInfoDto ToUserInfo(this SignInUpResponseModel model)
        {
            User user = new User()
            {
                Email = model.Email,
                Phone = model.Phone
            };
            UserInfoDto userInfoDto = new UserInfoDto() { Nickname = model.Nickname, User = user };

            return userInfoDto;
        }
    }
}
