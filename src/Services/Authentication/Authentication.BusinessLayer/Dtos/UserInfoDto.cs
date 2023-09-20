using Authentication.DomainLayer.Entities;

namespace Authentication.BusinessLayer.Dtos
{
    public class UserInfoDto : BaseEntity
    {
        public string? Nickname { get; set; }
        public Guid RoleId { get; set; }
        public UserRole? Role { get; set; }
        public User? User { get; set; }
        public Guid UserId { get; set; }
    }
}
