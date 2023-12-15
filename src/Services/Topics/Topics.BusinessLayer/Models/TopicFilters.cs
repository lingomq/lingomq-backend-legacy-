namespace Topics.BusinessLayer.Models
{
    public class TopicFilters
    {
        public Guid? LanguageId { get; set; }
        public Guid? LevelId { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.UnixEpoch;
        public DateTime? EndDate { get; set; } = DateTime.MaxValue;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = int.MaxValue;
        public int Count { get; set; } = int.MaxValue;
    }
}
