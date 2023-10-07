namespace Topics.DomainLayer.Entities
{
    public class Topic : BaseEntity
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Icon { get; set; }
        public DateTime CreationalDate { get; set; }
        public Guid LanguageId { get; set; }
        public Guid TopicLevelId { get; set; }
        public Language? Language { get; set; }
        public TopicLevel? TopicLevel { get; set; }
    }
}
