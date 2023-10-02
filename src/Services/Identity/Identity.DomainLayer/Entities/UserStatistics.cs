namespace Identity.DomainLayer.Entities
{
    public class UserStatistics : BaseEntity
    {
        public int TotalWords { get; set; }
        public float TotalHours { get; set; }
        public int VisitStreak { get; set; }
        public int AvgWords { get; set; }
        public DateTime LastUpdateAt { get; set; } 
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
