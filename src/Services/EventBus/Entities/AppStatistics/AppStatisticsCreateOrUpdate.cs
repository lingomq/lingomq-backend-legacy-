namespace EventBus.Entities.AppStatistics
{
    public class AppStatisticsCreateOrUpdate
    {
        public int TotalUsers { get; set; }
        public int TotalWords { get; set; }
        public int Downloads { get; set; }
        public DateTime Date { get; set; }
    }
}
