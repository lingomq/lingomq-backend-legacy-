namespace AppStatistics.DomainLayer.Entities
{
    public class StatisticsApp : BaseEntity
    {
        public int TotalUsers { get; set; }
        public int TotalWords { get; set; }
        public int Downloads { get; set; }
    }
}
