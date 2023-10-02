using System.ComponentModel.DataAnnotations;

namespace Identity.DomainLayer.Entities
{
    public class UserInfo : BaseEntity
    {
        [Required(ErrorMessage = "Поле nickname является обязательным полем")]
        [MaxLength(100, ErrorMessage = "Поле nickname не может содержать более 100 символов")]
        public string? Nickname { get; set; }
        public string? ImageUri { get; set; }
        [MaxLength(256, ErrorMessage = "Поле additional не может содержать более 256 символов")]
        public string? Additional { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public UserRole? Role { get; set; }
        public User? User { get; set; }
        public DateTime CreationalDate { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
