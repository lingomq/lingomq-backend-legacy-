using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.Entities;
public class UserRole : EntityBase
{
    [Required(ErrorMessage = "Поле name является обязательным полем")]
    [MaxLength(20, ErrorMessage = "Поле name не может содержать более 20 символов")]
    public string? Name { get; set; }
}
