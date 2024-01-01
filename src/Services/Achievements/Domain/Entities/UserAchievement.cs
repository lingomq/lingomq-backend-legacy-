namespace Achievements.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public User? User { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
