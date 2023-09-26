using Authentication.DomainLayer.Entities;
using Dapper.FluentMap.Mapping;

namespace Authentication.BusinessLayer.Services.Mapper
{
    public class UserInfoMapper : EntityMap<UserInfo>
    {
        public UserInfoMapper() 
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Nickname).ToColumn("nickname");
            Map(p => p.UserId).ToColumn("user_id");
            Map(p => p.RoleId).ToColumn("role_id");
            Map(p => p.Additional).ToColumn("additional");
            Map(p => p.CreationalDate).ToColumn("creational_date");
            Map(p => p.IsRemoved).ToColumn("is_removed");
            Map(p => p.ImageUri).ToColumn("image_uri");
        }
    }
}
