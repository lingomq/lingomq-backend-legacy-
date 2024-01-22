using Identity.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Identity.Domain.Dtos;
public class UserDto : EntityBase
{
    [Required(ErrorMessage = "Поле email является обязательным для заполнения")]
    [MaxLength(256, ErrorMessage = "Поле email не может быть больше 256 символов")]
    [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$",
        ErrorMessage = "Поле email является неверным")]
    public string? Email { get; set; }
    [MaxLength(15, ErrorMessage = "Неверный формат номера телефона")]
    [RegularExpression(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}",
        ErrorMessage = "Поле phone является неверным")]
    public string? Phone { get; set; }
}
