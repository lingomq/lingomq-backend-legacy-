namespace Identity.Domain.Entities;
public class UserStatistics : EntityBase
{
    public int TotalWords { get; set; } = 0;
    public float TotalHours { get; set; } = 0;
    public int VisitStreak { get; set; } = 0;
    public int AvgWords { get; set; } = 0;
    public DateTime LastUpdateAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
