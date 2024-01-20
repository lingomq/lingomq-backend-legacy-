namespace Words.Domain.Entities;
public class UserWordType : EntityBase
{
    public Guid UserWordId { get; set; }
    public Guid WordTypeId { get; set; }
    public UserWord? UserWord { get; set; }
    public WordType? WordType { get; set; }

}
