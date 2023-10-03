namespace Words.DomainLayer.Entities
{
    public class UserWordType : BaseEntity
    {
        public Guid UserWordId { get; set; }
        public Guid WordTypeId { get; set; }
        public UserWord? UserWord { get; set; }
        public WordType? WordType { get; set; }
    }
}
