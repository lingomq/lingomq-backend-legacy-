namespace Achievements.Domain.Entities
{
    public class UserAchievement : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
