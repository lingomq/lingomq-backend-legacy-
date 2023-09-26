using Authentication.DomainLayer.Entities;
using Dapper.FluentMap.Mapping;

namespace Authentication.BusinessLayer.Services.Mapper
{
    public class UserRoleMapper : EntityMap<UserRole>
    {
        public UserRoleMapper()
        {
            Map(p => p.Id).ToColumn("id");
            Map(p => p.Name).ToColumn("name");
        }
    }
}
