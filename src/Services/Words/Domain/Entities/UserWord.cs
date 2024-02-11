namespace Words.Domain.Entities;
public class UserWord : EntityBase
{
    public Word? Word { get; set; }
    public User? User { get; set; }
    public int Repeats { get; set; }
    public DateTime? AddedAt { get; set; }
}
