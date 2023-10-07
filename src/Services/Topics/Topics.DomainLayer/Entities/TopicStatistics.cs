namespace Topics.DomainLayer.Entities
{
    public class TopicStatistics : BaseEntity
    {
        public Guid TopicId { get; set; }
        public Guid UserId { get; set; }
        public Guid StatisticsTypeId { get; set; }
        public DateTime StatisticsDate { get; set; }
        public Topic? Topic { get; set; }
        public User? User { get; set; }
        public TopicStatisticsType? Type { get; set; }
    }
}
