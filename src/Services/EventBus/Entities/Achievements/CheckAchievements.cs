namespace EventBus.Entities.Achievements
{
    public class CheckAchievements
    {
        public Guid UserId { get; set; }
        public int WordsCount { get; set; }
        public DateTime RegisteredDate { get; set; } = DateTime.UnixEpoch;
    }
}
