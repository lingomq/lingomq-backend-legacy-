namespace Words.Domain.Entities;
public class UserWord : EntityBase
{
    public string? Word { get; set; }
    public string? Translated { get; set; }
    public Guid LanguageId { get; set; }
    public Guid UserId { get; set; }
    public int Repeats { get; set; }
    public DateTime CreatedAt { get; set; }
    public Language? Language { get; set; }
    public User? User { get; set; }

}
