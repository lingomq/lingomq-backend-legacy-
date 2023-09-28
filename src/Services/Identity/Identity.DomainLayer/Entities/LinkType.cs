using System.ComponentModel.DataAnnotations;

namespace Identity.DomainLayer.Entities
{
    public class LinkType : BaseEntity
    {
        [Required(ErrorMessage = "Поле name является обязательным полем")]
        [MaxLength(256, ErrorMessage = "Поле name не может содержать более 256 символов")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Поле short_link является обязательным полем")]
        [MaxLength(256, ErrorMessage = "Поле short_link не может содержать более 256 символов")]
        public string? ShortLink { get; set; }
    }
}
