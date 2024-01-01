namespace Achievements.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string? ImageUri { get; set; }
    }
}
