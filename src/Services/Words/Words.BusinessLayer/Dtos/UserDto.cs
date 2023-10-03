using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Dtos
{
    public class UserDto : BaseEntity
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
