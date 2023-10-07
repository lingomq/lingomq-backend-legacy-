namespace Topics.BusinessLayer.Dtos;

public class TopicStatisticsDto
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid UserId { get; set; }
    public Guid StatisticsTypeId { get; set; }
    public DateTime StatisticsDate { get; set; }
}