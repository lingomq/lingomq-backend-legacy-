using Words.DomainLayer.Entities;

namespace Words.BusinessLayer.Dtos
{
    public class UserWordDto : BaseEntity
    {
        public string? Word { get; set; }
        public string? Translated { get; set; }
        public Guid LanguageId { get; set; }
        public Guid UserId { get; set; }
    }
}
