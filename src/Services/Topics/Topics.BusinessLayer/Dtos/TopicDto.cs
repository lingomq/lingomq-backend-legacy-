namespace Topics.BusinessLayer.Dtos;

public class TopicDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Icon { get; set; }
    public DateTime CreationalDate { get; set; }
    public Guid LanguageId { get; set; }
    public Guid TopicLevelId { get; set; }
}