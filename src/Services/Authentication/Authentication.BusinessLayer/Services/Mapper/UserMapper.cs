using Authentication.DomainLayer.Entities;
using Dapper.FluentMap.Mapping;

namespace Authentication.BusinessLayer.Services.Mapper
{
    public class UserMapper : EntityMap<User>
    {
        public UserMapper()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Email).ToColumn("email");
            Map(p => p.Phone).ToColumn("phone");
            Map(p => p.PasswordSalt).ToColumn("password_salt");
            Map(p => p.PasswordHash).ToColumn("password_hash");
        }
    }
}
