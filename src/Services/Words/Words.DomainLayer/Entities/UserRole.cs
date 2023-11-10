using System.ComponentModel.DataAnnotations;

namespace Words.DomainLayer.Entities
{
    public class UserRole : BaseEntity
    {
        [Required(ErrorMessage = "Поле name является обязательным полем")]
        [MaxLength(20, ErrorMessage = "Поле name не может содержать более 20 символов")]
        public string? Name { get; set; }
    }
}
